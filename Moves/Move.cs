using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using System.Windows.Markup;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
//using Min = System.Math.Min;


namespace AkutenWars
{
    [Serializable]
    public class Move
    {
        public readonly Position startSquare;
        public readonly Position targetSquare;
        private Piece targetPiece = null;


        private int[][] numSquaresToEdge = new int[9 * 9][];

        Piece movedPiece;
        // public List<Piece> openedPieces { get; private set; } = new List<Piece>();
        public List<Position> openedPieces { get; private set; } = new List<Position>();
        public List<(Piece, Position)> removedPieces { get; private set; } = new List<(Piece, Position)>();

        public Move(Position from, Position oneMovePos)
        {
            this.startSquare = from;
            this.targetSquare = oneMovePos;
        }

        void PrecomputedMoveData()
        {

            for (int file = 0; file < 9; file++) //cols
            {
                for (int rank = 0; rank < 9; rank++) //rows
                {
                    int numNorth = 8 - rank;
                    int numSouth = rank;
                    int numWest = file;
                    int numEast = 8 - file;
                    int squareIndex = rank * 8 + file;

                    numSquaresToEdge[squareIndex] = new int[8];
                    int[] arr = {
                        numNorth ,numSouth ,numEast ,
                        Math.Min(numNorth, numWest),
                        Math.Min(numSouth, numEast),
                            Math.Min(numNorth, numEast),
                           Math. Min(numSouth, numWest)};
                    numSquaresToEdge[squareIndex] = arr;
                }



            }
        }

        /// <summary>
        /// I implemented it so that first the target Piece Sleeve gets opened. If it is a Landmine it expoldes without opening the other Pieces Sleeves.
        /// Then the attacking Piece Sleeve is opened. If it is a Landmine it explodes without opening the surrounding Pieces.
        /// Then  all Neighbour Pieces are opened. If one of these is a Landmine it explodes without opneing the Sleeves from its neighbours.If one of the neighbours is a horizontal Landmine, then the attacking and the target Piece gets destroyed and the attack end.
        /// If not the attack continues.
        /// </summary>
        /// <param name="board"></param>
        [Obsolete("Landmine and oppenedPiece doesnt work yet")]
        public void Execute(Board board)
        {
            Piece piece = board[startSquare];
            movedPiece = piece;
            board[startSquare] = null;
            targetPiece = board[targetSquare];
            //item.hasMoved = true; //mighbt be not necassary
            if (targetPiece == null)
            {
                board[targetSquare] = piece;
                return;
            }
            if (movedPiece.Sleeve.isOpen == false)
            {
                movedPiece.Sleeve.isOpen = true;
                // openedPieces.Add(movedPiece);
                openedPieces.Add(startSquare);
            }
            Card currentCard = piece.Card;
            piece.Sleeve.isOpen = true;
            Card targetCard = targetPiece.Card;
            if (targetPiece.Sleeve.isOpen == false)
            {
                targetPiece.Sleeve.isOpen = true;
                openedPieces.Add(targetSquare);
                //  openedPieces.Add(targetPiece);
            }


            //what happen if both are landmine?

            if (targetCard is Landmine)
            {
                Landmine lm = (Landmine)targetCard;
                // this.openedPieces.Add(targetPiece);
                // this.openedPieces.Add(targetSquare);
                IEnumerable<(Piece, Position)> rmoved = lm.Explode(board, targetSquare);
                this.removedPieces.AddRange(rmoved);

                return; //sleves are not opened
            }
            if (currentCard is Landmine)
            {
                Landmine lm = (Landmine)currentCard;
                //this.openedPieces.Add(startSquare);
                //    this.openedPieces.Add(movedPiece);
                IEnumerable<(Piece, Position)> rmoved = lm.Explode(board, targetSquare);
                this.removedPieces.AddRange(rmoved);
                return; //sleves are not opened
            }


            Dictionary<Piece, Position> neighbours2 = board.GetNeighbourDict(targetSquare);
            IEnumerable<KeyValuePair<Piece, Position>> kvpNeigbours = neighbours2.Where(x => x.Key != null); //not necassary

            //open Pieces
            foreach (KeyValuePair<Piece, Position> pp in neighbours2)
            {
                Piece p = pp.Key;
                if (p == null) continue;
                if (p.Sleeve.isOpen == false)
                {
                    p.Sleeve.isOpen = true;
                    openedPieces.Add(pp.Value);
                }
            }



            IEnumerable<KeyValuePair<Piece, Position>> lmsPieces = kvpNeigbours.Where(x => x.Key.Card is Landmine);

            foreach (KeyValuePair<Piece, Position> pp in lmsPieces)
            {
                Position lmSquare = pp.Value;
                Landmine lm = pp.Key.Card as Landmine;

                // openedPieces.Add(p);
                IEnumerable<(Piece, Position)> rmoved = lm.Explode(board, lmSquare);
                this.removedPieces.AddRange(rmoved);

            }

            // EnumPlayer opponentColor = item.Color == EnumPlayer.White ? EnumPlayer.Black : EnumPlayer.White;
            EnumPlayer opponentColor = piece.Color.Opponent();
            IEnumerable<Piece> currentNeighbours = kvpNeigbours.Where(x => x.Key.Color == piece.Color).Select(x => x.Key);
            IEnumerable<Piece> opponentNeighbours = kvpNeigbours.Where(x => x.Key.Color == opponentColor).Select(x => x.Key);

            int targetST = targetCard.ST;
            string info1 = $"{piece.FullName}\n{piece.Card}\nST={currentCard.ST}";
            string info2 = $"{targetPiece.FullName}\n{targetPiece.Card}\nST={targetCard.ST}";
            foreach (Piece opponentPiece in opponentNeighbours)
            {
                info2 += $"+{opponentPiece.Card.SP} /{opponentPiece.Card}\n";
                targetST += opponentPiece.Card.SP;


            }
            info2 += $"={targetST}";
            int currentST = currentCard.ST;
            foreach (Piece currentPiece in currentNeighbours)
            {
                info1 += $"+{currentPiece.Card.SP} /{currentPiece.Card}\n";
                currentST += currentPiece.Card.SP;
            }
            info1 += $"={currentST}";
            if (Math.Abs(currentST - targetST) < 1)
            {
                info1 += $"{currentCard.RPS.ToString()}";
                info2 += $"{targetCard.RPS.ToString()}";
                //do Rock Paper Scizzor
                if (currentCard.RPS > targetCard.RPS)
                {
                    currentST += 1;
                    info1 += "+1";
                }
                else
                {
                    targetST += 1;
                    info2 += "+1";
                }
            }
            string info = info1 + "\n" + info2;
            //MessageBox.Show(info, $"{piece.FullName} vs {targetPiece.FullName}");

            //if (currentST>targetST)
            //{
            //    item.Sleeve.isOpen = true;
            //}
            //else
            //{
            //    targetPiece.Sleeve.isOpen = true;
            //}
            board[targetSquare] = currentST > targetST ? piece : targetPiece;
            if (currentST > targetST)
            {
                this.removedPieces.Add((targetPiece, targetSquare));
            }
            else
            {
                this.removedPieces.Add((movedPiece, startSquare));
            }

        }


        public void Unmake(Board board)
        {
            foreach (var item in removedPieces)
            {
                Piece piece = item.Item1 as Piece;
                Position position = item.Item2 as Position;
                if (piece == null) continue;
                board[position] = piece;
            }
            board[startSquare] = movedPiece;
            if (targetPiece != null)
            {
                board[targetSquare] = targetPiece;
            }
            else
            {
                board[targetSquare] = null;
            }
            foreach (Position pos in openedPieces)
            {
                Piece p = board[pos];
                if (p is null) continue;
                p.Sleeve.isOpen = false;
            }



        }
        public override string ToString()
        {
            return $"{GetType().Name} {startSquare} -> {targetSquare}";
        }
        void importJson(string filePath)
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new Piece2DArrayConverter());
            string json = File.ReadAllText(filePath);
            Move Da = JsonConvert.DeserializeObject<Move>(json, settings);
            //this = Da;
        }

        public void saveJson(string filePath)
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new Piece2DArrayConverter());
            string json = JsonConvert.SerializeObject(this, Formatting.Indented, settings);
            File.WriteAllText(filePath, json);
        }

        public Move Copy()
        {
            Move newMove = new Move(startSquare, targetSquare);
            foreach ((Piece, Position) pp in this.removedPieces)
            {
                Piece p1 = pp.Item1 as Piece;
                Position p2 = pp.Item2 as Position;
                if (p1 != null)
                {
                    newMove.removedPieces.Add((p1.Copy(), p2));
                }

            }
            if (targetPiece != null)
            {
                newMove.targetPiece = targetPiece;
            }
            newMove.movedPiece=this.movedPiece.Copy();
            newMove.openedPieces.AddRange(this.openedPieces);
            return newMove;
        }
    }
}
