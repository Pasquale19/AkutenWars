using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
//using Min = System.Math.Min;


namespace AkutenWars
{
    public class Move
    {
        public Position startSquare { get; set; }
        public Position targetSquare { get; set; }

        private int[][] numSquaresToEdge = new int[9 * 9][];


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
        [Obsolete("move to board")]
        public void Execute(Board board)
        {
            Piece piece = board[startSquare];
            board[startSquare] = null;
            Piece targetPiece = board[targetSquare];
            piece.hasMoved = true; //mighbt be not necassary
            if (targetPiece == null)
            {
                board[targetSquare] = piece;
                return;
            }
            Card currentCard = piece.Card;
            piece.Sleeve.isOpen = true;
            Card targetCard = targetPiece.Card;
            targetPiece.Sleeve.isOpen = true;

            if (targetCard is Landmine)
            {
                Landmine lm = (Landmine)targetCard;
                lm.Detonate(board, targetSquare);
                return; //sleves are not opened
            }
            if (currentCard is Landmine)
            {
                Landmine lm = (Landmine)currentCard;
                lm.Detonate(board, targetSquare);
                return; //sleves are not opened
            }

            IEnumerable<Piece> neighbours = board.GetNeighbourPieces(targetSquare).Where(x => x != null);
            Dictionary<Piece, Position> neighbours2 = board.GetNeighbourDict(targetSquare);
            IEnumerable<KeyValuePair<Piece, Position>> kvpNeigbours = neighbours2.Where(x => x.Key != null); //not necassary
            IEnumerable<Piece> neighbourPieces = kvpNeigbours.Select(x => x.Key);
            foreach (Piece p in neighbourPieces) { p.Sleeve.isOpen = true; }


            
            IEnumerable<Piece> lmsPieces = neighbourPieces.Where(x => x.Card is Landmine);
           
            foreach (Piece p in lmsPieces)
            {
                Position lmSquare = neighbours2[p];
                Landmine lm = p.Card as Landmine;
                lm.Detonate(board, lmSquare);
            }

            if (board[targetSquare] == null) { return; }

            neighbours = board.GetNeighbourPieces(targetSquare).Where(x => x != null);
            // EnumPlayer opponentColor = piece.Color == EnumPlayer.White ? EnumPlayer.Black : EnumPlayer.White;
            EnumPlayer opponentColor = piece.Color.Opponent();
            IEnumerable<Piece> currentNeighbours = neighbours.Where(x => x.Color == piece.Color);
            IEnumerable<Piece> opponentNeighbours = neighbours.Where(x => x.Color == opponentColor);

            int targetST = targetCard.ST;
            string info1 = $"{piece.FullName}\n{piece.Card}\nST={currentCard.ST}";
            string info2 = $"{targetPiece.FullName}\n{targetPiece.Card}\nST={targetCard.ST}";
            foreach (Piece opponentPiece in opponentNeighbours)
            {
                info2 += $"+{opponentPiece.Card.SP} /{opponentPiece.Card}\n";
                targetST += opponentPiece.Card.SP;
                opponentPiece.Sleeve.isOpen = true;
            }
            info2 += $"={targetST}";
            int currentST = currentCard.ST;
            foreach (Piece currentPiece in currentNeighbours)
            {
                info1 += $"+{currentPiece.Card.SP} /{currentPiece.Card}\n";
                currentST += currentPiece.Card.SP;
                currentPiece.Sleeve.isOpen = true;
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
            MessageBox.Show(info, $"{piece.FullName} vs {targetPiece.FullName}");

            //if (currentST>targetST)
            //{
            //    piece.Sleeve.isOpen = true;
            //}
            //else
            //{
            //    targetPiece.Sleeve.isOpen = true;
            //}
            board[targetSquare] = currentST > targetST ? piece : targetPiece;

        }

        public override string ToString()
        {
            return $"{GetType().Name} {startSquare} -> {targetSquare}";
        }

    }
}
