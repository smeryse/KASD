using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ЛР1
{
    public abstract class Sport : Hobby
    {
        private uint _players_amount;
        private string _team_name;

        public Sport(string name, uint hardLvl, uint playersAmount, string teamName)
            : base(name, hardLvl)
        {
            PlayersAmount = playersAmount;
            TeamName = teamName;
        }

        public uint PlayersAmount
        {
            get { return _players_amount; }
            set { _players_amount = value; }
        }

        public string TeamName
        {
            get { return _team_name; }
            set { _team_name = value; }
        }
    }
}



