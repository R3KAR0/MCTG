using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.DataLayer.DTO
{
    public class Weakness
    {
        private readonly KindElementTypeDTO strong;

        public KindElementTypeDTO GetStrong()
        {
            return strong;
        }

        private readonly KindElementTypeDTO weak;

        public KindElementTypeDTO GetWeak()
        {
            return weak;
        }

        public Weakness(KindElementTypeDTO strong, KindElementTypeDTO weak)
        {
            this.strong = strong;
            this.weak = weak;    
        }
    }
}
