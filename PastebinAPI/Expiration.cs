﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace PastebinAPI {
    public class Expiration {
        public static readonly Expiration Never;
        public static readonly Expiration TenMinutes;
        public static readonly Expiration OneHour;
        public static readonly Expiration OneDay;
        public static readonly Expiration OneWeek;
        public static readonly Expiration TwoWeeks;
        public static readonly Expiration OneMonth;
        public static readonly Expiration Default;
        public static readonly IEnumerable<Expiration> All;

        static Expiration() {
            Never = new Expiration("N", TimeSpan.Zero);
            TenMinutes = new Expiration("10M", TimeSpan.FromMinutes(10));
            OneHour = new Expiration("1H", TimeSpan.FromHours(1));
            OneDay = new Expiration("1D", TimeSpan.FromDays(1));
            OneWeek = new Expiration("1W", TimeSpan.FromDays(7));
            TwoWeeks = new Expiration("2W", TimeSpan.FromDays(14));
            OneMonth = new Expiration("1M", TimeSpan.FromDays(30));
            Default = Never;
            All = getExpirations();
        }

        private static IEnumerable<Expiration> getExpirations() {
            yield return Never;
            yield return TenMinutes;
            yield return OneHour;
            yield return OneDay;
            yield return OneWeek;
            yield return TwoWeeks;
            yield return OneMonth;
        }

        public static Expiration Parse(string s) {
            Expiration result;
            if(s == null)
                throw new ArgumentNullException(nameof(s));
            if(TryParse(s, out result) == false)
                throw new FormatException($"Format: {s} is not supported");
            return result;
        }

        public static bool TryParse(string s, out Expiration result) {
            result = Default;
            if(s == null)
                return false;
            foreach(Expiration expiration in All) {
                if(s.ToUpper() != expiration.value) {
                    continue;
                }
                result = expiration;
                return true;
            }
            return false;
        }

        public static Expiration FromTimeSpan(TimeSpan timeSpan) {
            //if timespan <= 0 or > OneMonth then return never
            return All.FirstOrDefault(e => timeSpan <= e.time) ?? Never;
        }

        private readonly string value;
        private readonly TimeSpan time;

        private Expiration(string value, TimeSpan time) {
            this.value = value;
            this.time = time;
        }

        public TimeSpan Time => time;

        public override string ToString() {
            return value;
        }
    }
}
