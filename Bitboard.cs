using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bitboard = System.UInt64;
using Square = System.Int32;

namespace StockFishPortApp_12._0
{
    public struct Magic
    {
        Bitboard mask;
        Bitboard magic;
        Bitboard[] attacks;
        int shift;

        // Compute the attack's index using the 'magic bitboards' approach

        public uint index(Bitboard occupied) {
            return (uint)(((occupied & mask) * magic) >> shift);        
        }
    };
public class BitBoard
    {
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

        public const Bitboard QueenSide   = FileABB | FileBBB | FileCBB | FileDBB;
        public const Bitboard CenterFiles = FileCBB | FileDBB | FileEBB | FileFBB;
        public const Bitboard KingSide    = FileEBB | FileFBB | FileGBB | FileHBB;
        public const Bitboard Center      = (FileDBB | FileEBB) & (Rank4BB | Rank5BB);

        public static Bitboard[] KingFlank = new Bitboard[FileS.FILE_NB]{
            QueenSide ^ FileDBB, QueenSide, QueenSide,
            CenterFiles, CenterFiles,
            KingSide, KingSide, KingSide ^ FileEBB
        };

        public static ushort[] PopCnt16 = new ushort[1 << 16];
        public static ushort[][]  SquareDistance= new ushort[SquareS.SQUARE_NB][];
        public static Bitboard[] SquareBB = new Bitboard[SquareS.SQUARE_NB];
        public static Bitboard[][] LineBB = new Bitboard[SquareS.SQUARE_NB][];
        public static Bitboard[][] PseudoAttacks = new Bitboard[PieceTypeS.PIECE_TYPE_NB][];
        public static Bitboard[][] PawnAttackSpan = new Bitboard[ColorS.COLOR_NB][];

        public static Magic[] RookMagics= new Magic[SquareS.SQUARE_NB];
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
        public static Bitboard SquareXorBitboard(Bitboard b,  Square s)
        {
            return BitboardXorSquare(b, s);
        }

    }
}
