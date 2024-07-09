using GameServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Models
{
    public class Team
    {
        public int Leader { get; set; }

        public int TeamId { get; set; }

        public List<Character> Members { get; set; } = new List<Character>();
    }
}
