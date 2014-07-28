using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backup_Model
{
    [Serializable]
    public class Backup
    {
        private string _Name;
        public string Name { get { return _Name; } }

        public DateTime LastBackup;
        private string _Volume;
        public string Volume { get { return _Volume; } }
        public Backup(string name, string volume)
        {
            _Name = name;
            _Volume = volume;
            LastBackup = DateTime.MinValue;
        }
    }
}
