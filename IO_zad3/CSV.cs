using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace WindowsFormsApplication1
{
    class CSV
    {
        public char separator = ',';

        private StreamReader _reader;
        private ArrayList _collection;

        public CSV(string filename)
        {
            _reader = new StreamReader(filename);
        }

        public ArrayList GetCollection()
        {
            _collection = new ArrayList();

            string line = "";

            while ((line = _reader.ReadLine()) != null)
            {
                String[] row = line.Split(this.separator);
                _collection.Add(row);
            }

            return _collection;
        }
    }
}
