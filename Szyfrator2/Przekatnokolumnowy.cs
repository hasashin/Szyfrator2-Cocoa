using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace Szyfrator2
{

    public class Przekatnokolumnowy
    {
        public string Key { get; set; }

        public Przekatnokolumnowy() { }

        public Przekatnokolumnowy(string key)
        {
            Key = key;
        }

        private char[,] FillMatrix(string data)
        {
            CharEnumerator dataEnum = data.GetEnumerator();
            bool dataHasData = dataEnum.MoveNext();
            char[,] matrix = new char[Key.Length, (int)Math.Ceiling((decimal)data.Length / Key.Length)];
            for(int y= 0; y < matrix.GetLength(1); y++)
            {
                for(int x = 0; x < matrix.GetLength(0); x++)
                {
                    if (dataHasData)
                    {
                        matrix[x, y] = dataEnum.Current;
                        dataHasData = dataEnum.MoveNext();
                    }
                    else matrix[x, y] = '#';
                }
            }
            return matrix;
        }

        private List<int> CreateMap()
        {
            char[] keyChars = Key.ToCharArray();
            List<int> list = new List<int>();
            List<Tuple<int,int>> dict = new List<Tuple<int, int>>();
            for(int i=0;i<keyChars.Length;i++)
            {
                dict.Add(new Tuple<int,int>(keyChars[i],i));
            }
            var dictSorted = dict.OrderBy(x => x.Item1);
            foreach(var pair in dictSorted)
            {
                list.Add(pair.Item2);
            }
            return list;
        }

        private string CipherRead(char[,] input, List<int> map)
        {
            StringBuilder output = new StringBuilder();
            foreach (int startColumn in map)
            {
                int x = startColumn;
                for (int y = 0; y < input.GetLength(1); y++)
                {
                    output.Append(input[x, y]);
                    if (x == 0) x = input.GetLength(0)-1;
                    else x--;
                }
            }
            return output.ToString();
        }

        private char[,] CipherWrite(string data,List<int> map)
        {
            CharEnumerator charEnum = data.GetEnumerator();
            bool existsNextChar = charEnum.MoveNext();
            char[,] matrix = new char[Key.Length, (int)Math.Ceiling((decimal)data.Length / Key.Length)];
            foreach(int startColumn in map)
            {
                int x = startColumn;
                for (int y = 0; y < matrix.GetLength(1); y++)
                {
                    if(existsNextChar)
                        matrix[x, y] = charEnum.Current;
                    if (x == 0) x = matrix.GetLength(0)-1;
                    else x--;
                    existsNextChar = charEnum.MoveNext();
                }
            }
            return matrix;
        }

        public string Encrypt(string data)
        { 
            return CipherRead(FillMatrix(data), CreateMap());
        }

        public string Decrypt(string data)
        {
            var matrix = CipherWrite(data, CreateMap());
            StringBuilder msgBuilder = new StringBuilder();
            for(int y=0;y<matrix.GetLength(1);y++)
            {
                for(int x = 0; x < matrix.GetLength(0); x++)
                    msgBuilder.Append(matrix[x,y]);
            }
            return msgBuilder.ToString().Replace("#","");
        }
    }
}
