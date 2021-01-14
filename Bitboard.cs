using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bitboard = System.UInt64;
using Square = System.Int32;
using Rank = System.Int32;
using File = System.Int32;
using Direction = System.Int32;
using Color = System.Int32;
using PieceType = System.Int32;
using System.Numerics;

namespace StockFishPortApp_12._0
{
    public struct Magic
    {
        public Bitboard mask;
        public Bitboard magic;
        public Bitboard[] attacks;
        public int shift;

        // Compute the attack's index using the 'magic bitboards' approach

        public uint index(Bitboard occupied) {
            return (uint)(((occupied & mask) * magic) >> shift);        
        }
    };
    public class BitBoard
    {
        public const UInt64 DeBruijn_64 = 0x3F79D71B4CB0A89UL;
        public static int[] MS1BTable = new int[256];
        public static Square[] BSFTable = new Square[SquareS.SQUARE_NB];

        public const Bitboard AllSquares = ~0UL;
        public const Bitboard DarkSquares = 0xAA55AA55AA55AA55UL;
        public const Bitboard FileABB = 0x0101010101010101UL;
        public const Bitboard FileBBB = FileABB << 1;
        public const Bitboard FileCBB = FileABB << 2;
        public const Bitboard FileDBB = FileABB << 3;
        public const Bitboard FileEBB = FileABB << 4;
        public const Bitboard FileFBB = FileABB << 5;
        public const Bitboard FileGBB = FileABB << 6;
        public const Bitboard FileHBB = FileABB << 7;

        public const Bitboard Rank1BB = 0xFF;
        public const Bitboard Rank2BB = Rank1BB << (8 * 1);
        public const Bitboard Rank3BB = Rank1BB << (8 * 2);
        public const Bitboard Rank4BB = Rank1BB << (8 * 3);
        public const Bitboard Rank5BB = Rank1BB << (8 * 4);
        public const Bitboard Rank6BB = Rank1BB << (8 * 5);
        public const Bitboard Rank7BB = Rank1BB << (8 * 6);
        public const Bitboard Rank8BB = Rank1BB << (8 * 7);

        public const Bitboard QueenSide = FileABB | FileBBB | FileCBB | FileDBB;
        public const Bitboard CenterFiles = FileCBB | FileDBB | FileEBB | FileFBB;
        public const Bitboard KingSide = FileEBB | FileFBB | FileGBB | FileHBB;
        public const Bitboard Center = (FileDBB | FileEBB) & (Rank4BB | Rank5BB);

        public static Bitboard[] KingFlank = new Bitboard[FileS.FILE_NB]{
            QueenSide ^ FileDBB, QueenSide, QueenSide,
            CenterFiles, CenterFiles,
            KingSide, KingSide, KingSide ^ FileEBB
        };

        public static ushort[] PopCnt16 = new ushort[1 << 16];
        public static ushort[][] SquareDistance = new ushort[SquareS.SQUARE_NB][];
        public static Bitboard[] SquareBB = new Bitboard[SquareS.SQUARE_NB];
        public static Bitboard[][] LineBB = new Bitboard[SquareS.SQUARE_NB][];
        public static Bitboard[][] PseudoAttacks = new Bitboard[PieceTypeS.PIECE_TYPE_NB][];
        public static Bitboard[][] PawnAttacks = new Bitboard[ColorS.COLOR_NB][];

        public static Magic[] RookMagics = new Magic[SquareS.SQUARE_NB];
        public static Magic[] BishopMagics = new Magic[SquareS.SQUARE_NB];

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Bitboard square_bb(Square s)
        {
            Debug.Assert(Types.is_ok_square(s));
            return SquareBB[s];
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Bitboard BitboardAndSquare(Bitboard b, Square s)
        {
            return b & square_bb(s);
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Bitboard BitboardOrSquare(Bitboard b, Square s)
        {
            return b | square_bb(s);
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Bitboard BitboardXorSquare(Bitboard b, Square s)
        {
            return b ^ square_bb(s);
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Bitboard BitboardOrEqSquare(ref Bitboard b, Square s)
        {
            return b |= square_bb(s);
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Bitboard BitboardXorEqSquare(ref Bitboard b, Square s)
        {
            return b ^= square_bb(s);
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Bitboard SquareAndBitboard(Square s, Bitboard b)
        {
            return BitboardAndSquare(b, s);
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Bitboard SquareOrBitboard(Square s, Bitboard b)
        {
            return BitboardOrSquare(b, s);
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Bitboard SquareXorBitboard(Bitboard b, Square s)
        {
            return BitboardXorSquare(b, s);
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Bitboard SquareOrSquare(Square s1, Square s2)
        {
            return BitboardOrSquare(square_bb(s1), s2);
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool more_than_one(Bitboard b)
        {
            return (b & (b - 1)) != 0;
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool opposite_colors(Square s1, Square s2)
        {
            return ((s1 + Types.rank_of(s1) + s2 + Types.rank_of(s2)) & 1) != 0;
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Bitboard rank_bb_rank(Rank r)
        {
            return Rank1BB << (8 * r);
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Bitboard rank_bb_square(Square s)
        {
            return rank_bb_rank(Types.rank_of(s));
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Bitboard file_bb_file(File f)
        {
            return FileABB << f;
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Bitboard file_bb_square(Square s)
        {
            return file_bb_file(Types.file_of(s));
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Bitboard shift(Bitboard b, Direction D)
        {
            return D == DirectionS.NORTH ? b << 8 : D == DirectionS.SOUTH ? b >> 8
            : D == DirectionS.NORTH + DirectionS.NORTH ? b << 16 : D == DirectionS.SOUTH + DirectionS.SOUTH ? b >> 16
            : D == DirectionS.EAST ? (b & ~FileHBB) << 1 : D == DirectionS.WEST ? (b & ~FileABB) >> 1
            : D == DirectionS.NORTH_EAST ? (b & ~FileHBB) << 9 : D == DirectionS.NORTH_WEST ? (b & ~FileABB) << 7
            : D == DirectionS.SOUTH_EAST ? (b & ~FileHBB) >> 7 : D == DirectionS.SOUTH_WEST ? (b & ~FileABB) >> 9
            : 0;
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Bitboard pawn_attacks_bb(Bitboard b, Color C)
        {
            return C == ColorS.WHITE ? shift(b, DirectionS.NORTH_WEST) | shift(b, DirectionS.NORTH_EAST)
                    : shift(b, DirectionS.SOUTH_WEST) | shift(b, DirectionS.SOUTH_EAST);
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Bitboard pawn_attacks_bb(Color c, Square s)
        {
            Debug.Assert(Types.is_ok_square(s));
            return PawnAttacks[c][s];
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Bitboard pawn_double_attacks_bb(Bitboard b, Color C)
        {
            return C == ColorS.WHITE ? shift(b, DirectionS.NORTH_WEST) & shift(b, DirectionS.NORTH_EAST)
                    : shift(b, DirectionS.SOUTH_WEST) & shift(b, DirectionS.SOUTH_EAST);
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Bitboard adjacent_files_bb(Square s)
        {
            return shift(file_bb_square(s), DirectionS.EAST) | shift(file_bb_square(s), DirectionS.WEST);
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Bitboard line_bb(Square s1, Square s2)
        {
            Debug.Assert(Types.is_ok_square(s1) && Types.is_ok_square(s2));
            return LineBB[s1][s2];
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Bitboard between_bb(Square s1, Square s2)
        {
            Bitboard b = line_bb(s1, s2) & ((AllSquares << s1) ^ (AllSquares << s2));
            return b & (b - 1); //exclude lsb
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Bitboard forward_ranks_bb(Color c, Square s)
        {
            return c == ColorS.WHITE ? ~Rank1BB << 8 * Types.relative_rank_square(ColorS.WHITE, s)
                    : ~Rank8BB >> 8 * Types.relative_rank_square(ColorS.BLACK, s);
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Bitboard forward_file_bb(Color c, Square s)
        {
            return forward_ranks_bb(c, s) & file_bb_square(s);
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Bitboard pawn_attack_span(Color c, Square s)
        {
            return forward_ranks_bb(c, s) & adjacent_files_bb(s);
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Bitboard passed_pawn_span(Color c, Square s)
        {
            return pawn_attack_span(c, s) | forward_file_bb(c, s);
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool aligned(Square s1, Square s2, Square s3)
        {
            return BitboardAndSquare(line_bb(s1, s2), s3) != 0;
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static int distanceFile(Square x, Square y)
        {
            return Math.Abs(Types.file_of(x) - Types.file_of(y));
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static int distanceRank(Square x, Square y)
        {
            return Math.Abs(Types.rank_of(x) - Types.rank_of(y));
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static int distanceSquare(Square x, Square y)
        {
            return SquareDistance[x][y];
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static int edge_distanceFile(File f)
        {
            return Math.Min(f, (File)(FileS.FILE_H - f));
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static int edge_distanceRank(Rank r)
        {
            return Math.Min(r, (Rank)(RankS.RANK_8 - r));
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Bitboard attacks_bb(Square s, PieceType Pt)
        {
            Debug.Assert((Pt != PieceTypeS.PAWN) && (Types.is_ok_square(s)));
            return PseudoAttacks[Pt][s];
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Bitboard attacks_bb(Square s, Bitboard occupied, PieceType Pt)
        {
            Debug.Assert((Pt != PieceTypeS.PAWN) && (Types.is_ok_square(s)));

            switch (Pt)
            {
                case PieceTypeS.BISHOP: return BishopMagics[s].attacks[BishopMagics[s].index(occupied)];
                case PieceTypeS.ROOK: return RookMagics[s].attacks[RookMagics[s].index(occupied)];
                case PieceTypeS.QUEEN: return attacks_bb(s, occupied, PieceTypeS.BISHOP) | attacks_bb(s, occupied, PieceTypeS.ROOK);
                default: return PseudoAttacks[Pt][s];
            }
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Bitboard attacks_bb(PieceType pt, Square s, Bitboard occupied)
        {

            Debug.Assert((pt != PieceTypeS.PAWN) && (Types.is_ok_square(s)));

            switch (pt)
            {
                case PieceTypeS.BISHOP: return attacks_bb(s, occupied, PieceTypeS.BISHOP);
                case PieceTypeS.ROOK: return attacks_bb(s, occupied, PieceTypeS.ROOK);
                case PieceTypeS.QUEEN: return attacks_bb(s, occupied, PieceTypeS.BISHOP) | attacks_bb(s, occupied, PieceTypeS.ROOK);
                default: return PseudoAttacks[pt][s];
            }
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static int popcount(Bitboard b)
        {
            return BitOperations.PopCount(b);
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static uint bsf_index(Bitboard b)
        {            
            b ^= (b - 1);
            return (uint)((b * DeBruijn_64) >> 58);            
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Square lsb(Bitboard b)
        {
            return BSFTable[bsf_index(b)];
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Square msb(UInt64 b)
        {
            uint b32;
            int result = 0;

            if (b > 0xFFFFFFFF)
            {
                b >>= 32;
                result = 32;
            }

            b32 = (UInt32)(b);

            if (b32 > 0xFFFF)
            {
                b32 >>= 16;
                result += 16;
            }

            if (b32 > 0xFF)
            {
                b32 >>= 8;
                result += 8;
            }

            return (Square)(result + MS1BTable[b32]);
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Square pop_lsb(ref Bitboard b)
        {
            Debug.Assert(b!=0);
            Square s = lsb(b);
            b &= b - 1;
            return s;
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Square frontmost_sq(Color c, Bitboard b) 
        {
            Debug.Assert(b != 0);
            return c == ColorS.WHITE ? msb(b) : lsb(b); 
        }

        public static void init()
        {
            for (Square s = SquareS.SQ_A1; s <= SquareS.SQ_H8; ++s)
                BSFTable[bsf_index(SquareBB[s] = 1UL << s)] = s;

            for (Bitboard b = 1; b < 256; ++b)
                MS1BTable[b] = more_than_one(b) ? MS1BTable[b - 1] : lsb(b);
        }

#if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        

        
    }
}
