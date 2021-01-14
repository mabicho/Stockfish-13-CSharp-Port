using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Value = System.Int32;
using Piece = System.Int32;
using Score = System.Int32;
using Color = System.Int32;
using Square = System.Int32;
using CastlingRights = System.Int32;
using PieceType = System.Int32;
using File = System.Int32;
using Rank = System.Int32;
using Move = System.Int32;
using MoveType = System.Int32;
using Key = System.UInt64;


namespace StockFishPortApp_12
{


    public class Types
    {
        public const int MAX_MOVES = 256;
        public const int MAX_PLY = 246;

        public static Value[][] PieceValue = new Value[PhaseS.PHASE_NB][]{
            new Value[PieceS.PIECE_NB]{ ValueS.VALUE_ZERO, ValueS.PawnValueMg, ValueS.KnightValueMg, ValueS.BishopValueMg, ValueS.RookValueMg, ValueS.QueenValueMg, ValueS.VALUE_ZERO, ValueS.VALUE_ZERO, ValueS.VALUE_ZERO, ValueS.PawnValueMg, ValueS.KnightValueMg, ValueS.BishopValueMg, ValueS.RookValueMg, ValueS.QueenValueMg, ValueS.VALUE_ZERO, ValueS.VALUE_ZERO},
            new Value[PieceS.PIECE_NB]{ ValueS.VALUE_ZERO, ValueS.PawnValueEg, ValueS.KnightValueEg, ValueS.BishopValueEg, ValueS.RookValueEg, ValueS.QueenValueEg, ValueS.VALUE_ZERO, ValueS.VALUE_ZERO, ValueS.VALUE_ZERO, ValueS.PawnValueEg, ValueS.KnightValueEg, ValueS.BishopValueEg, ValueS.RookValueEg, ValueS.QueenValueEg, ValueS.VALUE_ZERO, ValueS.VALUE_ZERO}
        };

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Score make_score(int mg, int eg)
        {
            //return ((Int16)eg << 16) | (Int16)eg;
            return (Score)((int)((UInt32) eg << 16) +mg);
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Value eg_value(Score s)
        {
            return (Int16)((UInt32)(s + 0x8000) >> 16);          
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Value mg_value(Score s)
        {
            return  (Int16)((UInt32)(s)) ;
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Score divScore(Score s, int i)
        {
            return make_score(mg_value(s) / i, eg_value(s) / i);
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Score mulScore (Score s, int i) {

            Score result = (Score)((Int32)(s) * i);            

            Debug.Assert(eg_value(result) == (i* eg_value(s)));
            Debug.Assert(mg_value(result) == (i* mg_value(s)));
            Debug.Assert((i == 0) || (result / i) == s);

            return result;
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Score mulScore(Score s, bool b)
        {
            return b ? s : ScoreS.SCORE_ZERO;
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Color notColor(Color c)
        {
            return c ^ ColorS.BLACK;
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Square flip_rank(Square s)
        { // Swap A1 <-> A8
            return (Square)(s ^ SquareS.SQ_A8);
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Square flip_file(Square s)
        { // Swap A1 <-> H1
            return (Square)(s ^ SquareS.SQ_H1);
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Piece notPiece(Piece pc) {
            return (Piece)(pc ^ 8); // Swap color of piece B_KNIGHT <-> W_KNIGHT
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static CastlingRights andCastlingRights(Color c, CastlingRights cr) {
            return (CastlingRights)((c == ColorS.WHITE? CastlingRightS.WHITE_CASTLING : CastlingRightS.BLACK_CASTLING) & cr);
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Value mate_in(int ply)
        {
            return ValueS.VALUE_MATE - ply;
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Value mated_in(int ply)
        {
            return (Value)(-ValueS.VALUE_MATE + ply);
        }

#if AGGR_INLINE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Square make_square(File f, Rank r)
        {
            return ((r << 3) + f);
        }

#if AGGR_INLINE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Piece make_piece(Color c, PieceType pt)
        {
            return (Piece)((c << 3) + pt);
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static PieceType type_of_piece(Piece pc)
        {
            return (PieceType)(pc & 7);
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Color color_of(Piece pc)
        {
            Debug.Assert(pc != PieceS.NO_PIECE);
            return (Color)(pc >> 3);
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool is_ok_square(Square s)
        {
            return s >= SquareS.SQ_A1 && s <= SquareS.SQ_H8;
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static File file_of(Square s)
        {
            return (File)(s & 7);
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Rank rank_of(Square s)
        {
            return (Rank)(s >> 3);
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Square relative_square(Color c, Square s)
        {
            return (Square)(s ^ (c * 56));
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Rank relative_rank_rank(Color c, Rank r)
        {
            return (Rank)(r ^ (c * 7));
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Rank relative_rank_square(Color c, Square s)
        {
            return relative_rank_rank(c, rank_of(s));
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Square pawn_push(Color c)
        {
            return c == ColorS.WHITE ? DirectionS.NORTH : DirectionS.SOUTH;
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Square from_sq(Move m)
        {
            return ((m >> 6) & 0x3F);
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Square to_sq(Move m)
        {
            return (m & 0x3F);
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static int from_to(Move m)
        {
            return m & 0xFFF;
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static MoveType type_of_move(Move m)
        {
            return (MoveType)(m & (3 << 14));
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static PieceType promotion_type(Move m)
        {
            return (PieceType)(((m >> 12) & 3) + PieceTypeS.KNIGHT);
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Move make_move(Square from, Square to)
        {
            return (Move)((from << 6) + to);
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Move reverse_move(Move m)
        {
            return make_move(to_sq(m), from_sq(m));
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Move make(Square from, Square to, MoveType T, PieceType pt = PieceTypeS.KNIGHT)
        {            
            return (Move)(T + ((pt - PieceTypeS.KNIGHT) << 12) + (from << 6) + to);
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool is_ok_move(Move m)
        {
            return from_sq(m) != to_sq(m); // Catches also MOVE_NULL and MOVE_NONE
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Key make_key(UInt64 seed)
        {
            return seed * 6364136223846793005UL + 1442695040888963407UL;
        }
    }

    public struct MoveS
    {
        public const int MOVE_NONE = 0;
        public const int MOVE_NULL = 65;
    };

    public struct MoveTypeS
    {
        public const int NORMAL = 0;
        public const int PROMOTION = 1 << 14;
        public const int ENPASSANT = 2 << 14;
        public const int CASTLING = 3 << 14;
    };

    public struct ColorS
    {
        public const int WHITE = 0, BLACK = 1, COLOR_NB = 2;
    };

    public struct CastlingRightS
    {
        public const int NO_CASTLING = 0;
        public const int WHITE_OO = 1;
        public const int WHITE_OOO = WHITE_OO << 1;
        public const int BLACK_OO = WHITE_OO << 2;
        public const int BLACK_OOO = WHITE_OO << 3;

        public const int KING_SIDE = WHITE_OO | BLACK_OO;
        public const int QUEEN_SIDE = WHITE_OOO | BLACK_OOO;
        public const int WHITE_CASTLING = WHITE_OO | WHITE_OOO;
        public const int BLACK_CASTLING = BLACK_OO | BLACK_OOO;
        public const int ANY_CASTLING = WHITE_CASTLING | BLACK_CASTLING;

        public const int CASTLING_RIGHT_NB = 16;
    };

    public struct PhaseS
    {
        public const int PHASE_ENDGAME = 0;
        public const int PHASE_MIDGAME = 128;
        public const int MG = 0, EG = 1, PHASE_NB = 2;
    };

    public struct ScaleFactorS
    {
        public const int SCALE_FACTOR_DRAW = 0;
        public const int SCALE_FACTOR_NORMAL = 64;
        public const int SCALE_FACTOR_MAX = 128;
        public const int SCALE_FACTOR_NONE = 255;
    };

    public struct BoundS
    {
        public const int BOUND_NONE = 0;
        public const int BOUND_UPPER = 1;
        public const int BOUND_LOWER = 2;
        public const int BOUND_EXACT = BOUND_UPPER | BOUND_LOWER;
    };

    public struct ValueS
    {
        public const int VALUE_ZERO = 0;
        public const int VALUE_DRAW = 0;
        public const int VALUE_KNOWN_WIN = 10000;
        public const int VALUE_MATE = 32000;
        public const int VALUE_INFINITE = 32001;
        public const int VALUE_NONE = 32002;

        public const int VALUE_TB_WIN_IN_MAX_PLY = VALUE_MATE - 2 * Types.MAX_PLY;
        public const int VALUE_TB_LOSS_IN_MAX_PLY = -VALUE_TB_WIN_IN_MAX_PLY;
        public const int VALUE_MATE_IN_MAX_PLY = VALUE_MATE - Types.MAX_PLY;
        public const int VALUE_MATED_IN_MAX_PLY = -VALUE_MATE_IN_MAX_PLY;

        public const int PawnValueMg = 126, PawnValueEg = 208;
        public const int KnightValueMg = 781, KnightValueEg = 854;
        public const int BishopValueMg = 825, BishopValueEg = 915;
        public const int RookValueMg = 1276, RookValueEg = 1380;
        public const int QueenValueMg = 2538, QueenValueEg = 2682;
        public const int Tempo = 28;

        public const int MidgameLimit = 15258, EndgameLimit = 3915;
    };

    public struct PieceTypeS
    {
        public const int NO_PIECE_TYPE=0, PAWN=1, KNIGHT=2, BISHOP=3, ROOK=4, QUEEN=5, KING=6;
        public const int ALL_PIECES = 0;
        public const int PIECE_TYPE_NB = 8;
    };

    public struct PieceS
    {
        public const int NO_PIECE=0;
        public const int W_PAWN = 1, W_KNIGHT=2, W_BISHOP=3, W_ROOK=4, W_QUEEN=5, W_KING=6;
        public const int B_PAWN = 9, B_KNIGHT=10, B_BISHOP=11, B_ROOK=12, B_QUEEN=13, B_KING=14;
        public const int PIECE_NB = 16;
    };

    public struct DepthS
    {
        public const int DEPTH_QS_CHECKS = 0;
        public const int DEPTH_QS_NO_CHECKS = -1;
        public const int DEPTH_QS_RECAPTURES = -5;

        public const int DEPTH_NONE = -6;

        public const int DEPTH_OFFSET = -7;
    };

    public struct SquareS
    {
        public const int SQ_A1 = 0, SQ_B1 = 1, SQ_C1 = 2, SQ_D1 = 3, SQ_E1 = 4, SQ_F1 = 5, SQ_G1 = 6, SQ_H1 = 7;
        public const int SQ_A2 = 8, SQ_B2 = 9, SQ_C2 = 10, SQ_D2 = 11, SQ_E2 = 12, SQ_F2 = 13, SQ_G2 = 14, SQ_H2 = 15;
        public const int SQ_A3 = 16, SQ_B3 = 17, SQ_C3 = 18, SQ_D3 = 19, SQ_E3 = 20, SQ_F3 = 21, SQ_G3 = 22, SQ_H3 = 23;
        public const int SQ_A4 = 24, SQ_B4 = 25, SQ_C4 = 26, SQ_D4 = 27, SQ_E4 = 28, SQ_F4 = 29, SQ_G4 = 30, SQ_H4 = 31;
        public const int SQ_A5 = 32, SQ_B5 = 33, SQ_C5 = 34, SQ_D5 = 35, SQ_E5 = 36, SQ_F5 = 37, SQ_G5 = 38, SQ_H5 = 39;
        public const int SQ_A6 = 40, SQ_B6 = 41, SQ_C6 = 42, SQ_D6 = 43, SQ_E6 = 44, SQ_F6 = 45, SQ_G6 = 46, SQ_H6 = 47;
        public const int SQ_A7 = 48, SQ_B7 = 49, SQ_C7 = 50, SQ_D7 = 51, SQ_E7 = 52, SQ_F7 = 53, SQ_G7 = 54, SQ_H7 = 55;
        public const int SQ_A8 = 56, SQ_B8 = 57, SQ_C8 = 58, SQ_D8 = 59, SQ_E8 = 60, SQ_F8 = 61, SQ_G8 = 62, SQ_H8 = 63;
        public const int SQ_NONE = 64;

        public const int SQUARE_ZERO = 0;
        public const int SQUARE_NB = 64;
    };

    public struct DirectionS
    {
        public const int NORTH = 8;
        public const int EAST = 1;
        public const int SOUTH = -NORTH;
        public const int WEST = -EAST;

        public const int NORTH_EAST = NORTH + EAST;
        public const int SOUTH_EAST = SOUTH + EAST;
        public const int SOUTH_WEST = SOUTH + WEST;
        public const int NORTH_WEST = NORTH + WEST;
    };

    public struct FileS 
    {
        public const int FILE_A = 0, FILE_B = 1, FILE_C = 2, FILE_D = 3, FILE_E = 4, FILE_F = 5, FILE_G = 6, FILE_H = 7, FILE_NB = 8;
    };

    public struct RankS 
    {
        public const int RANK_1 = 0, RANK_2 = 1, RANK_3 = 2, RANK_4 = 3, RANK_5 = 4, RANK_6 = 5, RANK_7 = 6, RANK_8 = 7, RANK_NB = 8;    
    };

    public class DirtyPiece
    {        
        public int dirty_num;     
        public Piece[] piece = new Piece[3];
        public Square[] from = new Square[3];
        public Square[] to = new Square[3];
    };

    public struct ScoreS 
    {
        public const int SCORE_ZERO = 0;
    };


}
