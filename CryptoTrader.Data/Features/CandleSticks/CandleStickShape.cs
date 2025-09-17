using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrader.Data.Features.CandleSticks
{
    public enum CandleStickShape
    {
        None,

        Doji,
        DragonflyDoji,
        GravestoneDoji,

        Hammer,
        InvertedHammer,

        HangingMan,
        ShootingStar,

        BigWhiteCandle,
        BigBlackCandle,
        SpinningTop,
        Marubozu,
        ShavenHead,
        ShavenBottom
    }
}
