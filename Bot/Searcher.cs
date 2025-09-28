using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace AkutenWars.Bot
{
    public class Searcher
    {
        public static readonly Dictionary<string, int> PieceValues = new Dictionary<string, int>
    {
        { "K", 10000 },   // King
        { "R", 900 },     // Rook
        { "B", 800 },     // Bishop
        { "G", 600 },     // Gold General
        { "S", 500 },     // Silver General
        { "N", 300 },     // Knight
        { "L", 300 },     // Lance
        { "P", 100 },     // PawnStart
        { "+R", 950 },    // Promoted Rook
        { "+B", 850 },    // Promoted Bishop
        { "+S", 600 },    // Promoted Silver (moves like Gold)
        { "+N", 600 },    // Promoted Knight (moves like Gold)
        { "+L", 600 },    // Promoted Lance (moves like Gold)
        { "+P", 600 }     // Promoted PawnStart (moves like Gold)
    };

        protected static readonly Dictionary<Type, int> MaterialVal = new Dictionary<Type, int>
        {
            {typeof( King), 10000 },   // King
        { typeof(Rook), 900 },     // Rook
        { typeof(Bishop), 800 },     // Bishop
        { typeof(GoldGeneral), 600 },     // Gold General
        { typeof(SilverGeneral), 500 },     // Silver General
        { typeof(Knight), 300 },     // Knight
        { typeof(Lance), 300 },     // Lance
        {typeof(Pawn), 100 },     // PawnStart
        };



        [Obsolete("handle pieceColor")]
        public static int Evaluate(Board board, EnumPlayer color, int SupportBonus = 2)
        {
            //  IEnumerable<Piece> pieces = board.GetPieces();

            Piece[,] pieces = board.pieces;
            int rows = pieces.GetLength(0);
            int cols = pieces.GetLength(1);
            int sum = 0;
            const int openSleeve = -20;
            const int maxCardVal = 14;
            int anzPieces = board.GetPieces().Count();

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    Piece piece = pieces[r, c];
                    if (piece == null) continue;

                    //positional score

                    int sumPiece = 0;
                    int val = MaterialVal[piece.GetType()];
                    float phase = anzPieces / 40;
                    if (phase > 1 || phase < 0) throw new ArgumentException("phase must be between 0 and 1");
                    int positionVal = PieceSquareTable.PositionBonus(piece, r, c);
                    sumPiece += positionVal;
                    int Vz = piece.Color == color ? 1 : -1;
                    if (piece.Color == color)
                    {
                        Vz = 1;
                    }
                    else { Vz = -1; }

                    sumPiece += val;

                    if (piece.Sleeve.isOpen)
                    {
                        sumPiece += openSleeve; //is negative
                        Card card = piece.Card;
                        if (card is null) { throw new Exception(); }
                        if (card is Landmine)
                        {
                            sumPiece += maxCardVal;
                        }
                        else
                        {
                            int total = card.SP + card.ST;
                            if (card.RPS == RPS.Multi) total += 1;
                            sumPiece += total;
                        }

                    }
                    else
                    {
                        sumPiece += (int)(maxCardVal / 2.3);
                    }

                    sum += sumPiece * Vz;

                    int SupportPoints(Position pos)
                    {
                        if (!Board.IsInside(pos)) return 0;
                        Piece piece1 = board[pos];
                        if (piece1 is null) return 0;
                        if (piece1.Color == piece.Color) return SupportBonus;
                        return 0;
                    }
                    //supportBonus
                    List<Position> neighbours = new List<Position>(){
                        new Position(r-1,c),
                        new Position(r+1,c),
                        new Position(r,c-1),
                        new Position(r,c+1) };
                    for (int i = 0; i < 4; i++)
                    {
                        sum += SupportPoints(neighbours[i]) * Vz;
                    }
                }
            }



            return sum;
        }

        public Move FindBestMove(Board board, EnumPlayer isMaximizingPlayer, bool isMaximizingPlayer2, int MaxDepth = 1, bool NegaMax = true)
        {
            if (MaxDepth <= 0) throw new Exception("max Depth must e greater than 0");
            int alpha = int.MinValue;
            int beta = int.MaxValue;
            Move bestMove = null;
            // int bestScore = isMaximizingPlayer ? int.MinValue : int.MaxValue;
            int bestScore = int.MinValue;
            var moves = board.GenerateMoves(isMaximizingPlayer);
            foreach (var move in moves)
            {
                Board b1 = board.Copy();
                board.MakeMove(move);
                int score = 0;
                if (NegaMax)
                {
                    score = Negamax(board, MaxDepth - 1, alpha, beta, isMaximizingPlayer);
                }
                else
                {
                    score = AlphaBeta(board, MaxDepth - 1, alpha, beta, isMaximizingPlayer); //!isMaximizingPlayer
                }
                move.Unmake(board);//board.UndoMove(move);
                if (b1 != board)
                {
                    debugSave(b1, board, move);

                    throw new Exception("invalid Board");
                }
                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = move;
                }
                alpha = Math.Max(alpha, bestScore);

            }
            return bestMove;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="board"></param>
        /// <param name="depth"></param>
        /// <param name="alpha">best guaranteed value for the maximizer (highest minimum gain)</param>
        /// <param name="beta">best guaranteed value for the minimizer (lowest maximum loss)</param>
        /// <param name="isMaximizingPlayer"></param>
        /// <returns></returns>
        private int AlphaBeta(Board board, int depth, int alpha, int beta, EnumPlayer isMaximizingPlayer, bool maximizingPlayer = true)
        {
            GameResult result = board.Result();
            if (depth <= 0)
            {
                return Evaluate(board, isMaximizingPlayer);
            }
            if (result == GameResult.blackWon)
            {
                return isMaximizingPlayer == EnumPlayer.Black ? int.MaxValue : int.MinValue;
            }
            if (result == GameResult.whiteWon)
            {
                return isMaximizingPlayer == EnumPlayer.White ? int.MaxValue : int.MinValue;
            }
            if (result == GameResult.StaleMate)
            {
                return 0;
            }

            EnumPlayer color = maximizingPlayer ? isMaximizingPlayer : isMaximizingPlayer.Opponent();
            var moves = board.GenerateMoves(color);

            if (maximizingPlayer)
            {
                int maxEval = int.MinValue;
                foreach (var move in moves)
                {
                    // Board b2 = board.Copy();
                    board.MakeMove(move);
                    int eval = AlphaBeta(board, depth - 1, alpha, beta, isMaximizingPlayer, false); //false
                    //board = b2;
                    move.Unmake(board);//  board.UndoMove(move);
                    maxEval = Math.Max(maxEval, eval);
                    alpha = Math.Max(alpha, eval);

                    if (beta <= alpha) //maximizing Player will avoid this fork 
                        break; // beta cut-off
                }
                return maxEval;
            }
            else
            {
                int minEval = int.MaxValue;
                foreach (var move in moves)
                {
                    board.MakeMove(move);
                    int eval = AlphaBeta(board, depth - 1, alpha, beta, isMaximizingPlayer, true); //true
                    move.Unmake(board);//   board.UndoMove(move);
                    minEval = Math.Min(minEval, eval);
                    beta = Math.Min(beta, eval);

                    if (beta <= alpha)
                        break; // alpha cut-off
                }
                return minEval;
            }
        }
        int Negamax(Board board, int depth, int alpha, int beta, EnumPlayer color)
        {
            GameResult result = board.Result();
            if (depth <= 0)
            {
                return Evaluate(board, color);
            }
            if (result == GameResult.blackWon)
            {
                return color == EnumPlayer.Black ? int.MaxValue : int.MinValue;
            }
            if (result == GameResult.whiteWon)
            {
                return color == EnumPlayer.White ? int.MaxValue : int.MinValue;
            }
            if (result == GameResult.StaleMate)
            {
                return 0;
            }


            int maxValue = int.MinValue;
            var moves = board.GenerateMoves(color);
            foreach (Move move in moves)
            {
                Board b1 = board.Copy();
                board.MakeMove(move);
                int score = -Negamax(board, depth - 1, -beta, -alpha, color.Opponent());
                move.Unmake(board);
                if (!b1.Equals(board))
                {
                    debugSave(b1, board, move);
                    throw new Exception("board ungleich");
                }
                maxValue = Math.Max(maxValue, score);
                alpha = Math.Max(alpha, score);
                if (alpha >= beta)
                    break; // Beta cutoff
            }
            return maxValue;
        }

        void debugSave(Board expected, Board real, Move move)
        {
            string path = "Boards/";
            path = "";
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/";
            path = documentsPath;
            string file = "Board.bd";
            string file2 = "expectedBoard.bd";
            real.saveBinary(path + file);
            expected.saveBinary(path + file2);
            move.saveJson(path + "move.json");
        }

        //int Negamax(Board board, int depth, int alpha, int beta, int color)
        //{
        //    if (depth == 0 || board.IsGameOver())
        //        return color * Evaluate(board); // Evaluate from current player's perspective

        //    int maxValue = int.MinValue;
        //    foreach (Move move in board.GetPossibleMoves())
        //    {
        //        Board newBoard = board.ApplyMove(move);
        //        int score = -Negamax(newBoard, depth - 1, -beta, -alpha, -color);
        //        maxValue = Math.Max(maxValue, score);
        //        alpha = Math.Max(alpha, score);
        //        if (alpha >= beta)
        //            break; // Beta cutoff
        //    }
        //    return maxValue;
        //}
        //public Move FindBestMove2(Board board, EnumPlayer isMaximizingPlayer, bool isMaximizingPlayer2, int MaxDepth = 1)
        //{
        //    int alpha = int.MinValue;
        //    int beta = int.MaxValue;
        //    Move bestMove = null;
        //    int bestScore = isMaximizingPlayer ? int.MinValue : int.MaxValue;

        //    var moves = board.GenerateMoves(isMaximizingPlayer);
        //    foreach (var move in moves)
        //    {
        //        board.MakeMove(move);
        //        int score = AlphaBeta2(board, MaxDepth - 1, alpha, beta, isMaximizingPlayer.Opponent()); //!isMaximizingPlayer
        //        move.Unmake(board);//board.UndoMove(move);


        //        if (isMaximizingPlayer)
        //        {
        //            if (score > bestScore)
        //            {
        //                bestScore = score;
        //                bestMove = move;
        //            }
        //            alpha = Math.Max(alpha, bestScore);
        //        }
        //        else
        //        {
        //            if (score < bestScore)
        //            {
        //                bestScore = score;
        //                bestMove = move;
        //            }
        //            beta = Math.Min(beta, bestScore);
        //        }

        //        if (beta <= alpha)
        //            break; // pruning
        //    }
        //    return bestMove;
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="board"></param>
        ///// <param name="depth"></param>
        ///// <param name="alpha">best guaranteed value for the maximizer (highest minimum gain)</param>
        ///// <param name="beta">best guaranteed value for the minimizer (lowest maximum loss)</param>
        ///// <param name="isMaximizingPlayer"></param>
        ///// <returns></returns>
        //private int AlphaBeta2(Board board, int depth, int alpha, int beta, EnumPlayer isMaximizingPlayer, bool maximizingPlayer = true)
        //{
        //    GameResult result = board.Result();
        //    if (depth == 0)
        //    {
        //        return Evaluate(board, isMaximizingPlayer);
        //    }
        //    if (result == GameResult.blackWon)
        //    {
        //        return isMaximizingPlayer == EnumPlayer.Black ? int.MaxValue : int.MinValue;
        //    }
        //    if (result == GameResult.whiteWon)
        //    {
        //        return isMaximizingPlayer == EnumPlayer.White ? int.MaxValue : int.MinValue;
        //    }

        //    var moves = board.GenerateMoves(isMaximizingPlayer);

        //    if (maximizingPlayer)
        //    {
        //        int maxEval = int.MinValue;
        //        foreach (var move in moves)
        //        {
        //            // Board b2 = board.Copy();
        //            board.MakeMove(move);
        //            int eval = AlphaBeta(board, depth - 1, alpha, beta, isMaximizingPlayer.Opponent()); //false
        //            //board = b2;
        //            move.Unmake(board);//  board.UndoMove(move);
        //            maxEval = Math.Max(maxEval, eval);
        //            alpha = Math.Max(alpha, eval);

        //            if (beta <= alpha) //maximizing Player will avoid this fork 
        //                break; // beta cut-off
        //        }
        //        return maxEval;
        //    }
        //    else
        //    {
        //        int minEval = int.MaxValue;
        //        foreach (var move in moves)
        //        {
        //            board.MakeMove(move);
        //            int eval = AlphaBeta(board, depth - 1, alpha, beta, isMaximizingPlayer); //true
        //            move.Unmake(board);//   board.UndoMove(move);
        //            minEval = Math.Min(minEval, eval);
        //            beta = Math.Min(beta, eval);

        //            if (beta <= alpha)
        //                break; // alpha cut-off
        //        }
        //        return minEval;
        //    }
        //}
    }
}
