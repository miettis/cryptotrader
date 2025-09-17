namespace CryptoTrader.Data.Features.CandleSticks
{
    public class CandleStick
    {
        public CandleStick(CryptoStatistics statistics)
        {
            CandleStickExtensions.Initialize(statistics);
        }

        public CandleStickShape GetShape(Price price, CryptoStatistics statistics)
        {
            var trend = price.GetSlope();
            var downTrend = trend < 0;
            var upTrend = trend > 0;
            var xsBody = statistics.Body.Proportion.Lim10;
            var smBody = statistics.Body.Proportion.Lim25;
            var mdBody = statistics.Body.Proportion.Lim75;
            var lgBody = statistics.Body.Proportion.Lim90;
            var xsUpper = statistics.Upper.Proportion.Lim10;
            var xxsUpper = statistics.Upper.Proportion.Lim5;
            var xsLower = statistics.Lower.Proportion.Lim10;
            var xxsLower = statistics.Lower.Proportion.Lim5;

            if (price.Proportion.Body <= xsBody)
            {
                if (price.Proportion.Upper <= xsUpper)
                {
                    return CandleStickShape.DragonflyDoji;
                }
                if (price.Proportion.Lower <= xsLower)
                {
                    return CandleStickShape.GravestoneDoji;
                }
                return CandleStickShape.Doji;
            }
            else if (price.Proportion.Body <= smBody)
            {
                if (price.Proportion.Upper <= xsUpper && downTrend)
                {
                    return CandleStickShape.Hammer;
                }
                if (price.Proportion.Upper <= xsUpper && upTrend)
                {
                    return CandleStickShape.HangingMan;
                }
                if (price.Proportion.Lower <= xsLower && downTrend)
                {
                    return CandleStickShape.InvertedHammer;
                }
                if (price.Proportion.Lower <= xsLower && upTrend)
                {
                    return CandleStickShape.ShootingStar;
                }
            }
            else if (price.Proportion.Body <= mdBody)
            {
                if (Math.Abs(price.Proportion.Upper - price.Proportion.Lower) <= new[] { xsUpper, xsLower }.Average())
                {
                    return CandleStickShape.SpinningTop;
                }
            }
            else if (price.Proportion.Body >= lgBody)
            {
                if (price.Proportion.Upper <= xxsUpper && price.Proportion.Lower <= xxsLower)
                {
                    return CandleStickShape.Marubozu;
                }
                if (price.Proportion.Upper <= xxsUpper)
                {
                    return CandleStickShape.ShavenHead;
                }
                if (price.Proportion.Lower <= xxsLower)
                {
                    return CandleStickShape.ShavenBottom;
                }
            }

            return CandleStickShape.None;
        }
        
        public CandleStickPattern[][] GetAllPatterns(Price[] prices, decimal[] slopes = null)
        {
            var result = new CandleStickPattern[prices.Length][];
            for(var i = 0; i < prices.Length; i++)
            {
                var start = Math.Max(i - 4, 0);
                result[i] = GetPatterns(prices[start..(i + 1)], slopes).Select(x => x.Pattern).ToArray();
            }

            return result.ToArray();
        }
        public IEnumerable<(int Length, CandleStickPattern Pattern)> GetPatterns(Price[] prices, decimal[] slopes = null)
        {
            var patterns = new List<(int Length, CandleStickPattern Pattern)>();
            if (prices.Length >= 1)
            {
                patterns.AddRange(GetPatterns1(prices));
            }
            if (prices.Length >= 2)
            {
                patterns.AddRange(GetPatterns2(prices));
            }
            if (prices.Length >= 3)
            {
                patterns.AddRange(GetPatterns3(prices));
            }
            if (prices.Length >= 4)
            {
                patterns.AddRange(GetPatterns4(prices));
            }
            if (prices.Length >= 5)
            {
                patterns.AddRange(GetPatterns5(prices));
            }

            return patterns;


            // TODO: 8 New Price Lines
            // TODO: 10 New Price Lines
            // TODO: 12 New Price Lines
            // TODO: 13 New Price Lines
        }
        private IEnumerable<(int Length, CandleStickPattern Pattern)> GetPatterns1(Price[] prices, decimal[] slopes = null)
        {
            var price = prices.Last();
            var trend = slopes == null ? price.GetSlope() : slopes.Last();
            var upTrend = trend > 0;
            var downTrend = trend < 0;

            // Belt Hold, Bearish
            // Trend: Upward leading to the candle line
            // 1: Price opens at the high and closes near the low, creating a tall black candle
            if (upTrend &&
                price.Tall() && price.Black() && price.Tall() && price.OpeningHigh() && price.ClosingLow())
            {
                yield return (1, CandleStickPattern.BeltHoldBearish);
            }

            // Belt Hold, Bullish
            // Trend: Downward leading to the candlestick
            // 1: A tall white candle with price opening at the low and closing near the high
            if (downTrend &&
                price.Tall() && price.White() && price.OpeningLow() && price.ClosingHigh())
            {
                yield return (1, CandleStickPattern.BeltHoldBullish);
            }

            // Candle, Black
            // 1: A normal-size candle with shadows that do not exceed the body height
            if (price.Black() && price.NormalSize() && price.Length.Upper < price.Length.Body && price.Length.Lower < price.Length.Body)
            {
                yield return (1, CandleStickPattern.CandleBlack);
            }

            // Candle, Short Black
            // 1: A short candle with upper and lower shadows each shorter than the body
            if (price.Black() && price.Short() && price.Length.Upper < price.Length.Body && price.Length.Lower < price.Length.Body)
            {
                yield return (1, CandleStickPattern.CandleShortBlack);
            }

            // Candle, Short White
            // 1: A short candlestick with each shadow shorter than the bodyheight
            if (price.White() && price.Short() && price.Length.Upper < price.Length.Body && price.Length.Lower < price.Length.Body)
            {
                yield return (1, CandleStickPattern.CandleShortWhite);
            }

            // Candle, White
            // 1: A candle of normal size, neither short nor tall, with each shadow shorter than the body
            if (price.White() && price.NormalSize() && price.Length.Upper < price.Length.Body && price.Length.Lower < price.Length.Body)
            {
                yield return (1, CandleStickPattern.CandleWhite);
            }

            // Doji, Dragonfly
            // Trend:
            // 1: Price opens and closes at or near the high for the day while having a long lower shadow
            if (price.Doji() && price.Open.Near(price.High) && price.Close.Near(price.High) && price.NonExistentUpperShadow() && price.LongLowerShadow())
            {
                yield return (1, CandleStickPattern.DojiDragonfly);
            }

            // Doji, Gravestone
            // Trend:
            // 1: Price opens and closes at or very near the daily low with a tall upper shadow
            if (price.Doji() && price.Open.Near(price.Low) && price.Close.Near(price.Low) && price.NonExistentLowerShadow() && price.LongUpperShadow())
            {
                yield return (1, CandleStickPattern.DojiGravestone);
            }

            // Doji, Long - Legged
            // The opening and closing prices should be the same or nearly so, with long upper and lower shadows
            if (price.Doji() && price.Open.Near(price.Close) && price.LongUpperShadow() && price.LongLowerShadow())
            {
                yield return (1, CandleStickPattern.DojiLongLegged);
            }

            // Doji, Northern
            // Trend: Upward leading to the doji
            // A doji. Price opens and closes at or near the same price
            if (upTrend && price.Doji())
            {
                yield return (1, CandleStickPattern.DojiNorthern);
            }

            // Doji, Southern
            // Trend: Downward leading to the doji
            // A doji. The open and close are near in price to each other
            if (downTrend && price.Doji())
            {
                yield return (1, CandleStickPattern.DojiSouthern);
            }

            // Hammer
            // Trend: Downward leading to the candle pattern
            // 1: Has a lower shadow between two and three times the height of a small body and little or no upper shadow
            if (downTrend &&
                price.ShortBody() && price.Proportion.Lower >= 2 * price.Proportion.Body && price.Proportion.Lower <= 3 * price.Proportion.Body && price.NonExistentUpperShadow())
            {
                yield return (1, CandleStickPattern.Hammer);
            }

            // Hanging Man
            // Trend: Upward leading to the start of the hanging man
            // 1: Price opens at or near the high, forms a long lower shadow, and closes near, but not at, the high. A small body remains, either white or black, near the top of the trading range with a long lower shadow
            if (upTrend &&
                price.Open.Near(price.High) && price.LongLowerShadow() && price.Close.NearNotAt(price.High) && price.ShortUpperShadow())
            {
                yield return (1, CandleStickPattern.HangingMan);
            }

            // High Wave Bearish
            // Tall upper and lower shadows with a small body. Body can be either black or white
            if (price.ShortBody() && price.LongUpperShadow() && price.LongLowerShadow() && price.Close < price.Open)
            {
                yield return (1, CandleStickPattern.HighWaveBearish);
            }

            // High Wave Bullish
            // Tall upper and lower shadows with a small body. Body can be either black or white
            if (price.ShortBody() && price.LongUpperShadow() && price.LongLowerShadow() && price.Close > price.Open)
            {
                yield return (1, CandleStickPattern.HighWaveBullish);
            }


            if (prices.Length > 1)
            {
                // Long Black Day
                // The candle is black and the body height is at least three times the average body height of recent candles, with shadows shorter than the body
                if (price.Black() && price.Proportion.Body >= 3 * prices.Take(prices.Length - 1).Average(x => x.Proportion.Body) && price.Length.Upper < price.Length.Body && price.Length.Lower < price.Length.Body)
                {
                    yield return (1, CandleStickPattern.LongBlackDay);
                }

                // Long White Day
                // A tall white candle with a body three times the average of the prior week or two and with shadows shorter than the body
                if (price.White() && price.Proportion.Body >= 3 * prices.Take(prices.Length - 1).Average(x => x.Proportion.Body) && price.Length.Upper < price.Length.Body && price.Length.Lower < price.Length.Body)
                {
                    yield return (1, CandleStickPattern.LongWhiteDay);
                }
            }

            // Marubozu, Black
            // A tall black body with no shadows
            if (price.Marubozu() && price.Black() && price.Tall() && price.NonExistentUpperShadow() && price.NonExistentLowerShadow())
            {
                yield return (1, CandleStickPattern.MarubozuBlack);
            }

            // Marubozu, Closing Black
            // A tall black candle with an upper shadow but no lower shadow
            if (price.Marubozu() && price.Black() && price.Tall() && price.HasUpperShadow() && price.NonExistentLowerShadow())
            {
                yield return (1, CandleStickPattern.MarubozuClosingBlack);
            }

            // Marubozu, Closing White
            // A tall white body with a close at the high and a lower shadow
            if (price.Marubozu() && price.White() && price.Tall() && price.NonExistentUpperShadow() && price.HasLowerShadow())
            {
                yield return (1, CandleStickPattern.MarubozuClosingWhite);
            }

            // Marubozu, Opening Black
            // A tall black candle with a lower shadow but no upper shadow
            if (price.Marubozu() && price.Black() && price.Tall() && price.NonExistentUpperShadow() && price.HasLowerShadow())
            {
                yield return (1, CandleStickPattern.MarubozuOpeningBlack);
            }

            // Marubozu, Opening White
            // A tall white candle with an upper shadow but no lower one
            if (price.Marubozu() && price.White() && price.Tall() && price.HasUpperShadow() && price.NonExistentLowerShadow())
            {
                yield return (1, CandleStickPattern.MarubozuOpeningWhite);
            }

            // Marubozu, White
            // A tall white candle without shadows
            if (price.Marubozu() && price.White() && price.Tall() && price.NonExistentUpperShadow() && price.NonExistentLowerShadow())
            {
                yield return (1, CandleStickPattern.MarubozuWhite);
            }

            // Rickshaw Man
            // 1: A doji candle (open and close are the same price or nearly so) with the body near the middle of the candle and with exceptionally long shadows
            if (price.Doji() && price.ReallyLongUpperShadow() && price.ReallyLongLowerShadow())
            {
                yield return (1, CandleStickPattern.RickshawMan);
            }

            // Shooting Star, One - Candle
            // Trend: Upward leading to the start of the candle pattern
            // 1: tall upper shadow at least twice the body height above a small body. The body should be at or near the candle’s low, with no lower shadow(or a very small one)
            if (upTrend &&
                price.Length.Upper >= 2 * price.Length.Body && price.ShortBody() && price.NonExistentLowerShadow())
            {
                yield return (1, CandleStickPattern.ShootingStarOneCandle);
            }

            // Spinning Top, Black
            // 1: A small black body with shadows longer than the body
            if (price.ShortBody() && price.Black() && price.ShadowsLongerThanBody())
            {
                yield return (1, CandleStickPattern.SpinningTopBlack);
            }

            // Spinning Top, White
            // 1: A small white body with shadows longer than the body
            if (price.ShortBody() && price.White() && price.ShadowsLongerThanBody())
            {
                yield return (1, CandleStickPattern.SpinningTopWhite);
            }

            // Takuri Line
            // Trend: Downward leading to the start of the candle pattern
            // 1: A small candle body with no upper shadow, or a very small one, and a lower shadow at least three times the height of the body
            if (downTrend &&
                price.ShortBody() && price.NonExistentUpperShadow() && price.Length.Lower >= 3 * price.Length.Body)
            {
                yield return (1, CandleStickPattern.TakuriLine);
            }
        }
        private IEnumerable<(int Length, CandleStickPattern Pattern)> GetPatterns2(Price[] prices, decimal[] slopes = null)
        {
            var two = prices.TakeLast(2).ToArray();
            var trendStart = slopes == null ? two[0].GetSlope() : slopes.First();
            var upTrendStart = trendStart > 0;
            var downTrendStart = trendStart < 0;
            var trendEnd = slopes == null ? two[1].GetSlope() : slopes.Last();
            var upTrendEnd = trendEnd > 0;
            var downTrendEnd = trendEnd < 0;
            var price1 = two[0];
            var price2 = two[1];


            // Above the Stomach
            // Trend: Downward
            // 1: Black candle
            // 2: White candle opening and closing at or above the midpoint of the prior black candle’s body
            if (downTrendEnd &&
                price1.Black() &&
                price2.White() && price2.Open >= price1.Avg.OC && price2.Close >= price1.Avg.OC)
            {
                yield return (2, CandleStickPattern.AboveTheStomach);
            }

            // Below the Stomach
            // Trend: Upward leading to the start of the candlestick
            // 1: A tall white day
            // 2: The candle opens below the middle of the white candle’s body and closes at or below the middle, too
            if (upTrendStart &&
                price1.Tall() && price1.White() &&
                price2.Open < price1.Avg.OC && price2.Close <= price1.Avg.OC)
            {
                yield return (2, CandleStickPattern.BelowTheStomach);
            }

            // Dark Cloud Cover
            // Trend: Upward leading to the start of the candle pattern
            // 1: A tall white candle
            // 2: A black candle with the open above the prior high and a close below the midpoint of the prior day’s body
            if (upTrendStart &&
                price1.Tall() && price1.White() &&
                price2.Black() && price2.Open > price1.High && price2.Close < price1.Avg.OC)
            {
                yield return (2, CandleStickPattern.DarkCloudCover);
            }

            // Doji, Gapping Down
            // Trend: Downward (price gaps lower from the prior day)
            // 1: Price opens and closes at or near the same price for the day (a doji) but today’s upper shadow remains below the prior day’s lower shadow, leaving a price gap on the chart
            if (downTrendEnd &&
                price2.Doji() && price2.High < price1.Low)
            {
                yield return (2, CandleStickPattern.DojiGappingDown);
            }

            // Doji, Gapping Up
            // Trend: Upward (price gaps up from the prior day)
            // 1: Price opens and closes at or near the same price for the day (a doji), but today’s lower shadow remains above the prior day’s upper shadow, leaving a price gap on the chart
            if (upTrendEnd &&
                price2.Doji() && price2.Low > price1.High)
            {
                yield return (2, CandleStickPattern.DojiGappingUp);
            }

            // Doji Star, Bearish
            // Trend: Upward leading to the pattern
            // 1: A long white candle
            // 2: A doji. The open and close are at or near the same price
            // Price gaps higher, forming a body that is above the first day’s body
            // Avoid excessively long shadows on the doji. The sum of the doji shadows is less than the body height of the prior day
            if (price1.Tall() && price1.White() &&
                price2.Doji() &&
                price2.BodyAboveBody(price1) &&
                price2.Length.Upper + price2.Length.Lower < price1.Length.Body)
            {
                yield return (2, CandleStickPattern.DojiStarBearish);
            }

            // Doji Star, Bullish
            // Trend: Downward leading to the candle
            // 1: A tall black candle
            // 2: A doji. The opening and closing prices of the doji remain below the prior day’s close, even though the shadows may overlap.
            // The closing price of the first day is above the body of the doji
            // Discard patterns with unusually long doji shadows
            if (downTrendStart &&
                price1.Tall() && price1.Black() &&
                price2.Doji() && price2.Open < price1.Close && price2.Close < price1.Close &&
                price1.Close.AboveBody(price2) &&
                !price2.ReallyLongUpperShadow() && !price2.ReallyLongLowerShadow())
            {
                yield return (2, CandleStickPattern.DojiStarBullish);
            }

            // Engulfing, Bearish
            // Trend: Upward leading to the start of the candle pattern
            // 1: A white candle
            // 2: A black candle, the body of which overlaps the white candle’s body
            if (upTrendStart &&
                price1.White() &&
                price2.Black() && price2.BodyEngulfs(price1))
            {
                yield return (2, CandleStickPattern.EngulfingBearish);
            }

            // Engulfing, Bullish
            // Trend: Downward leading to the start of the candlestick pattern
            // 1: A black candle
            // 2: A white candle opens below the prior body and closes above that body, too. Price need not engulf the shadows
            if (downTrendStart &&
                price1.Black() &&
                price2.White() && price2.BodyEngulfs(price1))
            {
                yield return (2, CandleStickPattern.EngulfingBullish);
            }

            // Hammer, Inverted
            // Trend: Downward leading to the candle pattern
            // 1: A tall black candle with a close near the low of the day
            // 2: A small-bodied candle with a tall upper shadow and little or no lower shadow.Body cannot be a doji(otherwise it’s a gravestone doji).The open must be below the prior day’s close
            if (downTrendEnd &&
                price1.Tall() && price1.Black() && price1.Close.Near(price1.Low) &&
                price2.ShortBody() && price2.TallUpperShadow() && price2.NonExistentLowerShadow())
            {
                yield return (2, CandleStickPattern.HammerInverted);
            }

            // Harami, Bearish
            // Trend: Upward leading to the candle pattern
            // 1: A tall white candle
            // 2: A small black candle. The open and close must be within the body of the first day, but ignore the shadows. Either the tops or the bottoms of the bodies can be equal but not both
            if (upTrendEnd &&
                price1.Tall() && price1.White() &&
                price2.Short() && price2.Black() && price2.Open.WithinBody(price1) && price2.Close.WithinBody(price1))
            {
                yield return (2, CandleStickPattern.HaramiBearish);
            }

            // Harami, Bullish
            // Trend: Downward leading to the candle pattern
            // 1: A tall black candle
            // 2: A small-bodied white candle. The body must be within the prior candle’s body. The tops or bottoms of the two bodies can be the same price but not both
            if (downTrendEnd &&
                price1.Tall() && price1.Black() &&
                price2.Short() && price2.White() && price2.BodyWithinBody(price1))
            {
                yield return (2, CandleStickPattern.HaramiBullish);
            }

            // Harami Cross, Bearish
            // Trend: Upward leading to the start of the candle pattern
            // 1: A tall white candle
            // 2: A doji (open and close are equal or nearly so) with a trading range inside the price range of the prior day
            if (upTrendStart &&
                price1.Tall() && price1.White() &&
                price2.Doji() && price1.Engulfs(price2))
            {
                yield return (2, CandleStickPattern.HaramiCrossBearish);
            }

            // Harami Cross, Bullish
            // Trend: Downward leading to the start of the candle pattern
            // 1: A tall black candle
            // 2: A doji (open and closing prices are the same or nearly so) with a high - low price range that fits inside the range of the black candle
            if (downTrendStart &&
                price1.Tall() && price1.Black() &&
                price2.Doji() && price1.Engulfs(price2))
            {
                yield return (2, CandleStickPattern.HaramiCrossBullish);
            }

            // Homing Pigeon
            // Trend: Downward leading to the start of the candle pattern
            // 1: A tall black body
            // 2: A short black body that is inside the body of the first day
            if (downTrendStart &&
                price1.TallBody() && price1.Black() &&
                price2.ShortBody() && price2.Black() && price2.BodyWithinBody(price1))
            {
                yield return (2, CandleStickPattern.HomingPidgeon);
            }


            // In Neck
            // Trend: Downward leading to the start of the candle pattern
            // 1: A long black candle
            // 2: A white candle with an open below the low of the first day and a close that is into the body of the first day but not by much
            if (downTrendStart &&
                price1.Tall() && price1.Black() &&
                price2.White() && price2.Open < price1.Low && price2.Close > price1.Close && price2.Close.Near(price1.Close))
            {
                yield return (2, CandleStickPattern.InNeck);
            }

            // Kicking, Bearish
            // 1: A white marubozu candle, meaning a tall white candle with no shadows
            // 2: A black marubozu candle, meaning a tall black candle with no shadows. Price should gap below the white candle’s low
            if (downTrendEnd &&
                price1.White() && price1.Marubozu() &&
                price2.Black() && price2.Marubozu() && price2.High < price1.Low)
            {
                yield return (2, CandleStickPattern.KickingBearish);
            }

            // Kicking, Bullish
            // 1: A black marubozu candle: a tall black candle with no shadow
            // 2: Price gaps higher and a white marubozu candle forms: a tall white candle with no shadows
            if (downTrendEnd &&
                price1.Black() && price1.Marubozu() &&
                price2.White() && price2.Marubozu() && price2.Low > price1.High)
            {
                yield return (2, CandleStickPattern.KickingBullish);
            }

            // Last Engulfing Bottom
            // Trend: Downward leading to the start of the candle pattern
            // 1: A white candle
            // 2: A black candle opens above the prior body and closes below the prior body
            if (downTrendStart &&
                price1.White() &&
                price2.Black() && price2.Open.AboveBody(price1) && price2.Close.BelowBody(price1))
            {
                yield return (2, CandleStickPattern.LastEngulfingBottom);
            }

            // Last Engulfing Top
            // Trend: Upward leading to the start of the candle pattern
            // 1: A black candle
            // 2: A white candle, the body of which overlaps the prior black candle’s body
            if (upTrendStart &&
                price1.Black() &&
                price2.White() && price2.BodyEngulfs(price1))
            {
                yield return (2, CandleStickPattern.LastEngulfingTop);
            }


            // Matching Low
            // Trend: Downward leading to the start of the candle pattern
            // 1: A tall-bodied black candle
            // 2: A black body with a close that matches the prior clos
            if (downTrendStart &&
                price1.Tall() && price1.Black() &&
                price2.Black() && price2.Close.Near(price1.Close))
            {
                yield return (2, CandleStickPattern.MatchingLow);
            }

            // Meeting Lines, Bearish
            // Trend: Upward leading to the start of the candle pattern
            // 1: A tall white candle
            // 2: A tall black candle that closes at or near the prior day’s close
            if (upTrendStart &&
                price1.Tall() && price1.White() &&
                price2.Tall() && price2.Black() && price2.Close.Near(price1.Close))
            {
                yield return (2, CandleStickPattern.MeetingLinesBearish);
            }

            // Meeting Lines, Bullish
            // Trend: Downward leading to the start of the candle pattern
            // 1: A tall black candle
            // 2: A tall white candle with a closing price at or near the prior day’s close
            if (downTrendStart &&
                price1.Tall() && price1.Black() &&
                price2.Tall() && price2.White() && price2.Close.Near(price1.Close))
            {
                yield return (2, CandleStickPattern.MeetingLinesBullish);
            }

            // On Neck
            // Trend: Downward leading to the start of the candle pattern
            // 1: A tall black candle
            // 2: Price gaps lower but forms a white candle with a close that matches the prior low
            if (downTrendStart &&
                price1.Tall() && price1.Black() &&
                price2.Low < price1.Low && price2.White() && price2.Close.Near(price1.Low))
            {
                yield return (2, CandleStickPattern.OnNeck);
            }

            // Piercing Pattern
            // Trend: Downward leading to the start of the candle pattern
            // 1: A black candle
            // 2: A white candle that opens below the prior candle’s low and closes in the black body, between the midpoint and the open
            if (downTrendStart &&
                price1.Black() &&
                price2.White() && price2.Open < price1.Low && price2.Close > price1.Avg.OC && price2.Close < price1.Open)
            {
                yield return (2, CandleStickPattern.PiercingPattern);
            }

            // Separating Lines, Bearish
            // Trend: Downward leading to the start of the candle pattern
            // 1: A tall white candle
            // 2: A tall black candle that opens at or near the same price as the prior candle opened
            if (downTrendStart &&
                price1.Tall() && price1.White() &&
                price2.Tall() && price2.Black() && price2.Open.Near(price1.Open))
            {
                yield return (2, CandleStickPattern.SeparatingLinesBearish);
            }


            // Separating Lines, Bullish
            // Trend: Upward leading to the start of the candle pattern
            // 1: A tall black candle
            // 2: A tall white candle that opens at or near the prior day’s open
            if (upTrendStart &&
                price1.Tall() && price1.Black() &&
                price2.Tall() && price2.White() && price2.Open.Near(price1.Open))
            {
                yield return (2, CandleStickPattern.SeparatingLinesBullish);
            }

            // Shooting Star, Two - Candle
            // Trend: Upward leading to the start of the candle pattern
            // 1: A white candle
            // 2: A candle with an upper shadow at least three times the height of the body. The small body is at the bottom end of the candle and the candle has no lower shadow or a very small one.The body gaps above the prior day’s body
            if (upTrendStart &&
                price1.White() &&
                price2.Length.Upper >= 3 * price2.Length.Body && price2.ShortBody() && price2.NonExistentLowerShadow() && price2.BodyAboveBody(price1))
            {
                yield return (2, CandleStickPattern.ShootingStarTwoCandle);
            }

            // Thrusting
            // Trend: Downward leading to the start of the candle pattern
            // 1: A black candle
            // 2: A white candle opens below the prior low and closes near but below the midpoint of the prior body
            if (downTrendStart &&
                price1.Black() &&
                price2.White() && price2.Open < price2.Low && price2.Close.Near(price1.Avg.OC) && price2.Close < price2.Avg.OC)
            {
                yield return (2, CandleStickPattern.Thrusting);
            }

            // Tweezers Bottom
            // Trend: Downward leading to the start of the candle pattern
            // 1-2: Two candles (any color) that share the same low price
            if (downTrendStart &&
                price1.Low.Near(price2.Low))
            {
                yield return (2, CandleStickPattern.TweezersBottom);
            }

            // Tweezers Top
            // Trend: Upward leading to the start of the candle pattern
            // 1-2: Two adjacent candle lines (any color) share the same high price
            if (upTrendStart &&
                price1.High.Near(price2.High))
            {
                yield return (2, CandleStickPattern.TweezersTop);
            }



            // 
            // Trend:
            // 1: 
            // 2:
        }
        private IEnumerable<(int Length, CandleStickPattern Pattern)> GetPatterns3(Price[] prices, decimal[] slopes = null)
        {
            var three = prices.TakeLast(3).ToArray();
            var price1 = three[0];
            var price2 = three[1];
            var price3 = three[2];
            var trendStart = slopes == null ? three[0].GetSlope() : slopes.First();
            var upTrendStart = trendStart > 0;
            var downTrendStart = trendStart < 0;
            var trendEnd = slopes == null ? three[2].GetSlope() : slopes.Last();
            var upTrendEnd = trendEnd > 0;
            var downTrendEnd = trendEnd < 0;

            // Abandoned Baby, Bearish
            // Trend: Upward leading to the first candle
            // 1: A white candle, either short or tall
            // 2: A doji whose lower shadow gaps above the prior and following days’ highs(above their upper shadows)
            // 3: A black candle, either short or tall, with the upper shadow remaining below the doji’s lower shadow
            if (upTrendStart &&
                price1.White() &&
                price2.Doji() && price2.Low > price1.High && price2.Low > price3.High &&
                price3.Black())
            {
                yield return (3, CandleStickPattern.AbandonedBabyBearish);
            }

            // Abandoned Baby, Bullish
            // Trend: Downward leading to the start of the candlestick pattern
            // 1: Black candle
            // 2: Doji that gaps below the shadows of the candle lines on either side
            // 3: A white candle whose shadow gaps above the doji
            if (downTrendStart &&
                price1.Black() &&
                price2.Doji() && price2.High < price1.Low && price2.High < price3.Low &&
                price1.White())
            {
                yield return (3, CandleStickPattern.AbandonedBabyBullish);
            }

            // Advance block
            // Trend: Upward leading to the start of the candlestick pattern
            // 1: White
            // 2: White, Price must open within the previous body, Shadows taller than day 1
            // 3: White, Price must open within the previous body, Shadows taller than day 1
            if (upTrendStart &&
                price1.White() &&
                price2.White() && price2.Open.WithinBody(price1) && price2.TallerShadows(price1) &&
                price3.White() && price3.Open.WithinBody(price2) && price3.TallerShadows(price1))
            {
                yield return (3, CandleStickPattern.AdvanceBlock);
            }

            // Deliberation
            // Trend: Upward leading to the start of the candle pattern
            // 1: Long-bodied white candle
            // 2: Long-bodied white candle
            // 3: A small body that opens near the second day’s close
            // Each candle opens and closes higher than the previous ones’ opens and closes
            if (upTrendStart &&
                price1.Tall() && price1.White() &&
                price2.Tall() && price2.White() && price2.Open > price1.Open && price2.Close > price1.Close &&
                price3.ShortBody() && price3.Open.Near(price2.Close) && price3.Open > price2.Open && price3.Close > price2.Close)
            {
                yield return (3, CandleStickPattern.Deliberation);
            }

            // Doji Star, Collapsing
            // Trend: Upward leading to the start of the candle pattern
            // 1: A white candle
            // 2: A doji that gaps below yesterday’s candle, including the shadows
            // 3: A black day that gaps below the doji, including the shadows
            if (upTrendStart &&
                price1.White() &&
                price2.Doji() && price2.High < price1.Low &&
                price3.Black() && price3.High < price2.Low)
            {
                yield return (3, CandleStickPattern.DojiStarCollapsing);
            }

            // Downside Gap Three Methods
            // Trend: Downward leading to the candle pattern
            // 1: A long black-bodied candle
            // 2: Another long black-bodied candle with a gap between today and yesterday, including the shadows(meaning no overlapping shadows)
            // 3: Price forms a white candle. The candle opens within the body of the second day and closes within the body of the firstcandle, thus closing the gap between the two black candles
            if (downTrendEnd &&
                price1.Tall() && price1.Black() &&
                price2.Tall() && price2.Black() && price2.High < price1.Low &&
                price3.White() && price3.Open.WithinBody(price2) && price3.Close.WithinBody(price1))
            {
                yield return (3, CandleStickPattern.DownsideGapThreeMethods);
            }

            // Downside Tasuki Gap
            // Trend: Downward leading to the start of the candlestick pattern
            // 1: A black candle
            // 2: Price forms another black candle with a down gap between the first and second days. There should be no overlapping shadows
            // 3: Price forms a white candle that opens within the body of the second candle and closes within the gap set by the first and second day, but does not completely close the gap. Ignore the shadows
            if (downTrendStart &&
                price1.Black() &&
                price2.Black() && price2.High < price1.Low &&
                price3.White() && price3.Open.WithinBody(price2) && price3.Close > price2.Open && price3.Close < price1.Close)
            {
                yield return (3, CandleStickPattern.DownsideTasukiGap);
            }

            // Evening Doji Star
            // Trend: Upward leading to the start of the candle pattern
            // 1: A tall white day
            // 2: A doji that gaps above the bodies of the two adjacent candle lines. The shadows are not important; only the doji body need remain above the surrounding candles
            // 3: A tall black candle that closes at or below the midpoint (well into the body) of the white candle
            if (upTrendStart &&
            price1.Tall() && price1.White() &&
            price2.Doji() && price2.BodyAboveBody(price1) && price2.BodyAboveBody(price3) &&
            price3.Tall() && price3.Black() && price3.Close.Near(price1.Avg.OC) && price3.Close < price1.Avg.OC)
            {
                yield return (3, CandleStickPattern.EveningDojiStar);
            }

            // Evening Star
            // Trend: Upward leading to the start of the candle pattern
            // 1: A tall white candle
            // 2: A small-bodied candle that gaps above the bodies of the adjacent candles. It can be either black or white
            // 3: A tall black candle that gaps below the prior candle and closes at least halfway down the body of the white candle
            if (upTrendStart &&
                price1.Tall() && price1.White() &&
                price2.ShortBody() && price2.CompletelyAboveBody(price1) && price2.CompletelyAboveBody(price3) &&
                price3.Tall() && price3.Black() && price3.Close < price1.Avg.HL)
            {
                yield return (3, CandleStickPattern.EveningStar);
            }

            // Identical Three Crows
            // Trend: Upward leading to the start of the pattern
            // 1-3: Three tall black candles, the last two each opening at or near the prior close
            if (upTrendStart &&
                price1.Tall() && price1.Black() &&
                price2.Tall() && price2.Black() && price2.Open.Near(price1.Close) &&
                price3.Tall() && price3.Black() && price3.Open.Near(price2.Close))
            {
                yield return (3, CandleStickPattern.IdenticalThreeCrows);
            }

            // Morning Doji Star
            // Trend: Downward leading to the start of the candle pattern
            // 1: A tall black candle
            // 2: A doji whose body gaps below the prior body
            // 3: A tall white candle whose body remains above the doji’s body
            if (downTrendStart &&
                price1.Tall() && price1.Black() &&
                price2.Doji() && price2.BodyBelowBody(price1) &&
                price3.Tall() && price3.White() && price3.BodyAboveBody(price2))
            {
                yield return (3, CandleStickPattern.MorningDojiStar);
            }

            // Morning Star
            // Trend: Downward leading to the start of the candle pattern
            // 1: A tall black candl
            // 2: A small-bodied candle that gaps lower from the prior body
            // 3: A tall white candle that gaps above the body of the second day and closes at least midway into the black body of the first day
            if (downTrendStart &&
                price1.Tall() && price1.Black() &&
                price2.ShortBody() && price2.BodyBelowBody(price1) &&
                price3.Tall() && price3.White() && price3.BodyAboveBody(price2) && price3.Close > price1.Avg.OC)
            {
                yield return (3, CandleStickPattern.MorningStar);
            }

            // Side-by-Side White Lines, Bearish
            // Trend: Downward leading to the start of the candle patter
            // 1: A black candle
            // 2-3: White candles with bodies about the same size and opening prices near the same value.The closing prices in both candles remain below the body of the black candle
            if (downTrendStart &&
                price1.Black() &&
                price2.White() && price2.Close.BelowBody(price1) &&
                price3.White() && price3.Close.BelowBody(price1) && price3.Length.Body.Near(price2.Length.Body) && price3.Open.Near(price2.Open))
            {
                yield return (3, CandleStickPattern.SideBySideWhiteLinesBearish);
            }

            // Side-by-Side White Lines, Bullish
            // Trend: Upward leading to the start of the candle pattern
            // 1: A white candle
            // 2-3: Two white candles with bodies of similar size and opening prices near each other. The bodies of both candles remain above the body of the first white candle, leaving a gap
            if (downTrendStart &&
                price1.White() &&
                price2.White() && price2.BodyAboveBody(price1) &&
                price3.White() && price3.BodyAboveBody(price1) && price3.Length.Body.Near(price2.Length.Body) && price3.Open.Near(price2.Open))
            {
                yield return (3, CandleStickPattern.SideBySideWhiteLinesBullish);
            }

            // Stick Sandwich
            // Trend: Downward leading to the start of the candle pattern
            // 1: A black candle
            // 2: A white candle that trades above the close of the first day
            // 3: A black candle that closes at or near the close of the first day
            if (downTrendStart &&
                price1.Black() &&
                price2.White() && price2.CompletelyAbove(price1.Close) &&
                price3.Black() && price3.Close.Near(price1.Close))
            {
                yield return (3, CandleStickPattern.StickSandwich);
            }

            // Three Black Crows
            // Trend: Upward leading to the start of the candle pattern
            // 1-3: Three tall black candles, each closing at a new low. The last two candles open within the body of the previous candle. All should close at or near their lows
            if (upTrendStart &&
                price1.Tall() && price1.Black() && price1.Close.Near(price1.Low) &&
                price2.Tall() && price2.Black() && price2.Close < price1.Close && price2.Open.WithinBody(price1) && price2.Close.Near(price2.Low) &&
                price3.Tall() && price3.Black() && price3.Close < price2.Close && price3.Open.WithinBody(price2) && price3.Close.Near(price3.Low))
            {
                yield return (3, CandleStickPattern.ThreeBlackCrows);
            }

            // Three Inside Down
            // Trend: Upward leading to the start of the candle pattern
            // 1: A tall white candle
            // 2: A small black candle. The open and close must be within the body of the first day, but ignore the shadows. Either the tops or the bottoms of the bodies can be equal, but not both
            // 3: Price must close lower
            if (upTrendStart &&
                price1.Tall() && price1.White() &&
                price2.Short() && price2.Black() && price2.Open.WithinBody(price1) && price2.Close.WithinBody(price1) &&
                price3.Close < price2.Close)
            {
                yield return (3, CandleStickPattern.ThreeInsideDown);
            }

            // Three Inside Up
            // Trend: Downward leading to the start of the candle pattern
            // 1: A tall black candle
            // 2: A small-bodied white candle. The body must be within the prior candle’s body. The tops or bottoms of the two bodies can be the same price but not both
            // 3: A white candle that closes above the prior day’s close
            if (downTrendStart &&
                price1.Tall() && price1.Black() &&
                price2.ShortBody() && price2.White() && price2.BodyWithinBody(price1) &&
                price3.White() && price3.Close > price2.Close)
            {
                yield return (3, CandleStickPattern.ThreeInsideUp);
            }

            // Three Outside Down
            // Trend: Upward leading to the start of the candle pattern
            // 1: A white candle
            // 2: A black candle opens higher and closes lower than the prior candle’s body, engulfing it
            // 3: A candle with a lower close
            if (upTrendStart &&
                price1.White() &&
                price2.Black() && price2.Open.AboveBody(price1) && price2.Close.BelowBody(price1) &&
                price3.Close < price2.Close)
            {
                yield return (3, CandleStickPattern.ThreeOutsideDown);
            }

            // Three Outside Up
            // Trend: Downward leading to the start of the candle pattern
            // 1: A black candle
            // 2: A white candle opens below the prior body and closes above the body, too.Price need not engulf the shadows
            // 3: A white candle in which price closes higher
            if (downTrendStart &&
                price1.Black() &&
                price2.White() && price2.Open.BelowBody(price1) && price2.Close.AboveBody(price1) &&
                price3.White() && price3.Close > price2.Close)
            {
                yield return (3, CandleStickPattern.ThreeOutsideUp);
            }

            // Three Stars in the South
            // Trend: Downward leading to the start of the candle
            // 1: A tall black candle with a long lower shadow
            // 2: Similar to the first day but smaller and with a low above the previous day’s low
            // 3: A black marubozu type candle fits inside the high-low trading range of the prior day
            if (downTrendStart &&
                price1.Tall() && price1.Black() && price1.LongLowerShadow() &&
                price2.Tall() && price2.Black() && price2.LongLowerShadow() && price2.Length.Candle < price1.Length.Candle && price2.Low > price1.Low &&
                price3.Black() && price3.Marubozu() && price3.BodyWithin(price2))
            {
                yield return (3, CandleStickPattern.ThreeStarsInTheSouth);
            }

            // Three White Soldiers
            // Trend: Downward leading to the start of the candle pattern
            // 1-3: Tall white candles with higher closes and price that opens within the previous body. Price should close near the high each day
            if (downTrendStart &&
                price1.Tall() && price1.White() && price1.Close.Near(price1.High) &&
                price2.Tall() && price2.White() && price2.Close > price1.Close && price2.Open.WithinBody(price1) && price2.Close.Near(price2.High) &&
                price3.Tall() && price3.White() && price3.Close > price2.Close && price3.Open.WithinBody(price2) && price3.Close.Near(price3.High))
            {
                yield return (3, CandleStickPattern.ThreeWhiteSoldiers);
            }


            // Tri-Star, Bearish
            // Trend: Upward leading to the start of the candle pattern
            // 1-3: Look for three doji, the middle one has a body above the other two
            if (upTrendStart &&
                price1.Doji() &&
                price2.Doji() && price2.BodyAboveBody(price1) && price2.BodyAboveBody(price3) &&
                price3.Doji())
            {
                yield return (3, CandleStickPattern.TriStarBearish);
            }


            // Tri-Star, Bullish
            // Trend: Downward leading to the start of the candle pattern
            // 1-3: Three doji. The middle one has a body below the other two 
            if (downTrendStart &&
                price1.Doji() &&
                price2.Doji() && price2.BodyBelowBody(price1) && price2.BodyBelowBody(price3) &&
                price3.Doji())
            {
                yield return (3, CandleStickPattern.TriStarBullish);
            }

            // Two Black Gapping Candles
            // Trend: Downward leading to the start of the candle pattern
            // 1: Price gaps lower from the prior day and forms a black candle
            // 2: A lower high forms on the second black candle
            if (downTrendStart &&
               price2.Black() && price2.High < price1.Low &&
               price3.Black() && price3.High < price2.High)
            {
                yield return (3, CandleStickPattern.TwoBlackGappingCandles);
            }

            // Two Crows
            // Trend: Upward leading to the start of the candle pattern
            // 1: A tall white candle
            // 2: A black candle with a body that gaps above the prior body
            // 3: A black candle that opens within the prior body and closes within the white candle’s body(first day).
            if (upTrendStart &&
                price1.Tall() && price1.White() &&
                price2.Black() && price2.BodyAboveBody(price1) &&
                price3.Black() && price3.Open.WithinBody(price2) && price3.Close.WithinBody(price1))
            {
                yield return (3, CandleStickPattern.TwoCrows);
            }

            // Unique Three-River Bottom
            // Trend: Downward leading to the start of the candle pattern
            // 1: A tall-bodied black candle
            // 2: The black body is inside the prior body, but the low price (long lower shadow) is below the prior day’s low
            // 3: A short-bodied white candle, which is below the body of the prior day
            if (downTrendStart &&
                price1.TallBody() && price1.Black() &&
                price2.Black() && price2.BodyWithinBody(price1) && price2.Low < price1.Low &&
                price3.ShortBody() && price3.White() && price3.BodyBelowBody(price2))
            {
                yield return (3, CandleStickPattern.UniqueThreeRiverBottom);
            }

            // Upside Gap Three Methods
            // Trend: Upward leading to the start of the candle pattern
            // 1-2: Two tall white candles with a gap between them, even between the shadows
            // 3: A black candle fills the gap
            if (upTrendStart &&
                price1.Tall() && price1.White() &&
                price2.Tall() && price2.White() && price2.CompletelyAbove(price1) &&
                price3.Black() && price3.High > price2.Low && price3.Low < price1.High)
            {
                yield return (3, CandleStickPattern.UpsideGapThreeMethods);
            }

            // Upside Gap Two Crows
            // Trend: Upward leading to the start of the candle pattern
            // 1: A tall white candle
            // 2: A black candle with a body gapping above the prior candle’s body
            // 3: A black candle that engulfs the body of the prior day. The close remains above the close of the first day
            if (upTrendStart &&
                price1.Tall() && price1.White() &&
                price2.Black() && price2.BodyAboveBody(price1) &&
                price3.Black() && price3.EngulfsBody(price2) && price3.Close > price1.Close)
            {
                yield return (3, CandleStickPattern.UpsideGapTwoCrows);
            }


            // Upside Tasuki Gap
            // Trend: Upward leading to the start of the candle pattern
            // 1: A white candle
            // 2: A white candle. Price gaps higher, including the shadows, leaving a rising window between the two candles
            // 3: A black candle opens in the body of the prior candle and closes within the gap.The gap remains open if you ignore the lower shadow
            if (upTrendStart &&
                price1.White() &&
                price2.White() && price2.CompletelyAbove(price1) &&
                price3.Black() && price3.Open.WithinBody(price2) && price3.Close < price2.Open && price3.Close > price1.Close)
            {
                yield return (3, CandleStickPattern.UpsideTasukiGap);
            }
        }
        private IEnumerable<(int Length, CandleStickPattern Pattern)> GetPatterns4(Price[] prices, decimal[] slopes = null)
        {
            var four = prices.TakeLast(4).ToArray();
            var price1 = four[0];
            var price2 = four[1];
            var price3 = four[2];
            var price4 = four[3];
            var trendStart = slopes == null ? four[0].GetSlope() : slopes.First();
            var upTrendStart = trendStart > 0;
            var downTrendStart = trendStart < 0;
            var trendEnd = slopes == null ? four[3].GetSlope() : slopes.Last();
            var upTrendEnd = trendEnd > 0;
            var downTrendEnd = trendEnd < 0;

            // Concealing Baby Swallow
            // Trend: Downward leading to the start of the candle pattern 
            // 1&2: Two long black candles without any shadows (both are black marubozu candles)
            // 3: A black day with a tall upper shadow. The candle gaps open downward and yet trades into the body of the prior day
            // 4: Another black day that completely engulfs the prior day, including the shadows
            if (downTrendStart &&
                price1.Tall() && price1.Black() && price1.NonExistentUpperShadow() && price1.NonExistentLowerShadow() &&
                price2.Tall() && price2.Black() && price2.NonExistentUpperShadow() && price2.NonExistentLowerShadow() &&
                price3.Black() && price2.LongUpperShadow() && price3.Open < price2.Close && price2.High.WithinBody(price2) &&
                price4.Black() && price4.Engulfs(price3))
            {
                yield return (4, CandleStickPattern.ConcealingBabySwallow);
            }

            // Three-Line Strike, Bearish
            // Trend: Downward leading to the start of the candle pattern
            // 1-3: Three black candles form lower closes
            // 4: A white candle opens below the prior close and closes above the first day’s open
            if (downTrendStart &&
                price1.Black() &&
                price2.Black() && price2.Close < price1.Close &&
                price3.Black() && price3.Close < price2.Close &&
                price4.White() && price4.Open < price3.Close && price4.Close > price1.Open)
            {
                yield return (4, CandleStickPattern.ThreeLineStrikeBearish);
            }

            // Three-Line Strike, Bullish
            // Trend: Upward leading to the start of the candle pattern
            // 1-3: Three white candles, each with a higher close
            // 4: A black candle that opens higher but closes below the open of the first candle
            if (upTrendStart &&
                price1.White() &&
                price2.White() && price2.Close > price1.Close &&
                price3.White() && price3.Close > price2.Close &&
                price4.Black() && price4.Open > price3.Close && price4.Close < price1.Open)
            {
                yield return (4, CandleStickPattern.ThreeLineStrikeBullish);
            }
        }
        private IEnumerable<(int Length, CandleStickPattern Pattern)> GetPatterns5(Price[] prices, decimal[] slopes = null)
        {
            // 
            // Trend: 
            // 1: 
            // 2: 
            // 3: 
            // 4: 
            // 5: 

            var five = prices.TakeLast(5).ToArray();
            var price1 = five[0];
            var price2 = five[1];
            var price3 = five[2];
            var price4 = five[3];
            var price5 = five[4];
            var trendStart = slopes == null ? five[0].GetSlope() : slopes.First();
            var upTrendStart = trendStart > 0;
            var downTrendStart = trendStart < 0;
            var trendEnd = slopes == null ? five[4].GetSlope() : slopes.Last();
            var upTrendEnd = trendEnd > 0;
            var downTrendEnd = trendEnd < 0;

            // Breakaway, Bearish
            // Trend: Upward leading to the start of the candle pattern
            // 1: A tall white candle
            // 2: A white candle that has a gap between the two candle bodies, but the shadows can overlap
            // 3: A candle with a higher close
            // 4: A white candle with a higher close
            // 5: A tall black candle with a close within the gap between the first two candle bodies.Ignore the shadows on the first two candles for citing the gap
            if (upTrendStart &&
                price1.Tall() && price1.White() &&
                price2.White() && price2.BodyAboveBody(price1) &&
                price3.Close > price2.Close &&
                price4.White() & price4.Close > price3.Close &&
                price5.Tall() && price5.Black() && price5.Close.AboveBody(price1) && price2.Close.BelowBody(price2))
            {
                yield return (5, CandleStickPattern.BreakawayBearish);
            }

            // Breakaway, Bullish
            // Trend: Downward leading to the start of the candle pattern
            // 1: A tall black candle
            // 2: A black candle that has a gap between the two candle bodies, but the shadows can overlap
            // 3: A candle with a lower close
            // 4: A black candle with a lower close
            // 5: A tall white candle with a close within the gap between the first two candles.Ignore the shadows on the first two candles for citing the gap
            if (downTrendStart &&
                price1.Tall() && price1.Black() &&
                price2.Black() && price2.BodyBelowBody(price1) &&
                price3.Close < price2.Close &&
                price4.Black() & price4.Close < price3.Close &&
                price5.Tall() && price5.White() && price5.Close.BelowBody(price1) && price2.Close.AboveBody(price2))
            {
                yield return (5, CandleStickPattern.BreakawayBullish);
            }

            // Falling Three Methods
            // Trend: Downward leading to the start of the candle pattern
            // 1: A tall black candle
            // 2-4: Short candles, the middle one of which can be either black or white but the others are white. The three trend upward but remain within the high - low range of the first day
            // 5: A tall black candle with a close below the first day’s close
            if (downTrendStart &&
                price1.Tall() && price1.Black() &&
                price2.Short() && price2.Close.WithinBody(price1) && price2.White() && price2.Close > price1.Close &&
                price3.Short() && price3.Close.WithinBody(price1) && price3.Close > price2.Close &&
                price4.Short() && price4.Close.WithinBody(price1) && price4.White() && price4.Close > price3.Close && price4.Close < price1.High &&
                price5.Tall() && price5.Black() && price5.Close < price1.Close)
            {
                yield return (5, CandleStickPattern.FallingThreeMethods);
            }

            // Ladder Bottom
            // Trend: Downward leading to the start of the candle pattern
            // 1-3: Tall black candles, each with a lower open and lower close
            // 4: A black candle with an upper shadow
            // 5: A white candle that gaps above the body of the prior day
            if (downTrendStart &&
                price1.Tall() && price1.Black() &&
                price2.Tall() && price2.Black() && price2.Open < price1.Open && price2.Close < price1.Close &&
                price3.Tall() && price3.Black() && price3.Open < price2.Open && price3.Close < price2.Close &&
                price4.Black() && price2.HasUpperShadow() &&
                price5.White() && price2.CompletelyAboveBody(price4))
            {
                yield return (5, CandleStickPattern.LadderBottom);
            }

            // Mat Hold
            // Trend: Upward leading to the start of the candle pattern
            // 1: A long white body
            // 2: Price gaps open upward but closes lower (meaning a small black candle) and yet remains above the prior close.Ignore the shadows
            // 3: Small body with the closing price easing lower, but the body remain above the low of the first day
            // 4: Small black body. Closing price easing lower, but the body remain above the low of the first day
            // 5: A white candle with a close above the highs of the prior four candles
            if (upTrendStart &&
                price1.Tall() && price1.White() &&
                price2.Black() && price2.Open > price1.Close && price2.Close > price1.Close &&
                price3.ShortBody() && price3.BodyAbove(price1.Low) &&
                price4.Black() && price4.ShortBody() && price4.BodyAbove(price1.Low) &&
                price5.White() && price5.Close > five.Take(4).Max(x => x.High))
            {
                yield return (5, CandleStickPattern.MatHold);
            }

            // Rising Three Methods
            // Trend: Upward leading to the start of the candle pattern
            // 1: A tall white-bodied candle
            // 2-4: Small-bodied candles that trend lower and close within the high - low range of the first candle.Day 3 can be black or white, but days 2 and 4 are black candles
            // 5: A tall white candle that closes above the close of the first day
            if (upTrendStart &&
                price1.Tall() && price1.White() &&
                price2.ShortBody() && price2.Close.WithinBody(price1) && price2.Black() && price2.Close < price1.Close &&
                price3.ShortBody() && price3.Close.WithinBody(price1) && price3.Close < price2.Close &&
                price4.ShortBody() && price4.Close.WithinBody(price1) && price4.Black() && price4.Close < price3.Close &&
                price5.Tall() && price5.White() && price5.Close > price1.Close)
            {
                yield return (5, CandleStickPattern.RisingThreeMethods);
            }
        }
    }

    public static class CandleStickExtensions
    {
        private static readonly decimal nearDiff = 0.001m;
        private static CryptoStatistics stats;

        public static void Initialize(CryptoStatistics statistics)
        {
            stats = statistics;
        }

        /// <summary>
        /// <= 25%
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public static bool Short(this Price price)
        {
            var lim = stats.Candle.Length.Lim25;
            return price.Length.Candle <= lim;
        }

        /// <summary>
        /// 25% - 75%
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public static bool NormalSize(this Price price)
        {
            return !(price.Short() || price.Tall());
        }
        /// <summary>
        /// >= 75%
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public static bool Tall(this Price price)
        {
            var lim = stats.Candle.Length.Lim75;
            return price.Length.Candle >= lim;
        }


        /// <summary>
        /// <= 10%
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public static bool Doji(this Price price)
        {
            var lim = stats.Body.Length.Lim10;
            return price.Length.Body <= lim;
        }
        /// <summary>
        /// <= 25%
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public static bool ShortBody(this Price price)
        {
            var lim = stats.Body.Length.Lim25;
            return !price.Doji() && price.Length.Body <= lim;
        }
        /// <summary>
        /// >= 75%
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public static bool TallBody(this Price price)
        {
            var lim = stats.Body.Length.Lim75;
            return price.Length.Body >= lim;
        }
        public static bool LongUpperShadow(this Price price)
        {
            var lim = stats.Upper.Length.Lim75;
            return price.Length.Upper >= lim;
        }
        public static bool ReallyLongUpperShadow(this Price price)
        {
            var lim = stats.Upper.Length.Lim90;
            return price.Length.Upper >= lim;
        }
        public static bool LongLowerShadow(this Price price)
        {
            var lim = stats.Lower.Length.Lim75;
            return price.Length.Lower >= lim;
        }
        public static bool ReallyLongLowerShadow(this Price price)
        {
            var lim = stats.Lower.Length.Lim90;
            return price.Length.Lower >= lim;
        }
        public static bool TallerShadows(this Price price, Price other)
        {
            return price.Proportion.Upper > other.Proportion.Upper && price.Proportion.Lower > other.Proportion.Lower;
        }

        public static bool Marubozu(this Price price)
        {
            return price.NonExistentUpperShadow() && price.NonExistentLowerShadow();
        }

        // Candle colors
        public static bool Black(this Price price)
        {
            return price.Close < price.Open;
        }
        public static bool White(this Price price)
        {
            return price.Close > price.Open;
        }

        // 
        public static bool WithinBody(this decimal value, Price other)
        {
            var min = Math.Min(other.Open, other.Close);
            var max = Math.Max(other.Open, other.Close);
            return min <= value && value <= max;
        }

        public static bool OpeningHigh(this Price price)
        {
            return price.Open > (1 - nearDiff) * price.High;
        }
        public static bool OpeningLow(this Price price)
        {
            return price.Open < (1 + nearDiff) * price.Low;
        }
        public static bool ClosingHigh(this Price price)
        {
            return price.Close > (1 - nearDiff) * price.High;
        }
        public static bool ClosingLow(this Price price)
        {
            return price.Close < (1 + nearDiff) * price.Low;
        }

        public static bool Near(this decimal value, decimal other)
        {
            return Math.Abs((value - other) / value) < nearDiff;
        }
        public static bool Near(this double value, double other)
        {
            return Math.Abs((value - other) / value) < (double)nearDiff;
        }
        public static bool NearNotAt(this decimal value, decimal other)
        {
            return value.Near(other) && value != other;
        }
        public static bool BodyAbove(this Price price, decimal value)
        {
            return price.Open > value && price.Close > value;
        }
        public static bool BodyAboveBody(this Price price, Price other)
        {
            var max = Math.Max(other.Open, other.Close);
            return price.Open > max && price.Close > max;
        }
        public static bool BodyBelowBody(this Price price, Price other)
        {
            var min = Math.Min(other.Open, other.Close);
            return price.Open < min && price.Close < min;
        }

        public static bool CompletelyAbove(this Price price, decimal value)
        {
            return price.Low > value;
        }
        public static bool CompletelyAbove(this Price price, Price other)
        {
            return price.Low > other.High;
        }
        public static bool CompletelyAboveBody(this Price price, Price other)
        {
            var max = Math.Max(other.Open, other.Close);
            return price.Low > max;
        }
        public static bool AboveBody(this decimal value, Price price)
        {
            return value > price.Open && value > price.Close;
        }
        public static bool BelowBody(this decimal value, Price price)
        {
            return value < price.Open && value < price.Close;
        }
        public static bool BodyEngulfs(this Price price, Price other)
        {
            return
                Math.Min(price.Open, price.Close) <= Math.Min(other.Open, other.Close) &&
                Math.Max(price.Open, price.Close) >= Math.Max(other.Open, other.Close);
        }
        public static bool BodyWithinBody(this Price price, Price other)
        {
            var min = Math.Min(other.Open, other.Close);
            var max = Math.Max(other.Open, other.Close);

            return min <= price.Open && price.Open <= max &&
                   min <= price.Close && price.Close <= max;
        }
        public static bool BodyWithin(this Price price, Price other)
        {
            return other.Low <= price.Open && price.Open <= other.High &&
                   other.Low <= price.Close && price.Close <= other.High;
        }
        public static bool Engulfs(this Price price, Price other)
        {
            return price.High > other.High && price.Low < other.Low;
        }
        public static bool EngulfsBody(this Price price, Price other)
        {
            var min = Math.Min(other.Open, other.Close);
            var max = Math.Max(other.Open, other.Close);
            return price.High > max && price.Low < min;
        }

        public static bool Upward(this decimal value)
        {
            return value > 0;
        }
        public static bool Downward(this decimal value)
        {
            return value < 0;
        }

        public static bool NonExistentUpperShadow(this Price price)
        {
            var lim = stats.Upper.Proportion.Lim10;
            return price.Proportion.Upper <= lim;
        }
        public static bool NonExistentLowerShadow(this Price price)
        {
            var lim = stats.Lower.Proportion.Lim10;
            return price.Proportion.Lower <= lim;
        }
        public static bool ShortUpperShadow(this Price price)
        {
            var lim = stats.Upper.Proportion.Lim25;
            return price.Proportion.Upper <= lim;
        }
        public static bool ShadowsLongerThanBody(this Price price)
        {
            return price.Length.Upper > price.Length.Body && price.Length.Lower > price.Length.Body;
        }
        public static bool TallUpperShadow(this Price price)
        {
            var lim = stats.Upper.Proportion.Lim75;
            return price.Proportion.Upper >= lim;
        }
        public static bool HasUpperShadow(this Price price)
        {
            var lim = stats.Upper.Proportion.Lim25;
            return price.Proportion.Upper >= lim;
        }
        public static bool HasLowerShadow(this Price price)
        {
            var lim = stats.Lower.Proportion.Lim25;
            return price.Proportion.Lower >= lim;
        }
    }
}
