/*  
*/
using System;
using System.Collections.Generic;
using System.Linq;

/* Handle three issues.
 * 1. Show Balance.
 * 2. IsBalanced().
 * 3. Reverse().
 * 
 * */

namespace FNTC.Finansoft.Accounting.BLL
{
    public class PartidaDoble : IList<Anotacion>
    {
        private List<Anotacion> entries;
        private decimal totDebit;
        private decimal totCredit;
        private decimal balance;
        private decimal total;

        public PartidaDoble()
        {
            totDebit = decimal.Zero;
            totCredit = decimal.Zero;
            balance = decimal.Zero;
            entries = new List<Anotacion>();
        }

        public decimal Balance
        {
            get
            {
                return balance;
            }
        }


        public decimal Credito { get { return this.totCredit; } }

        public decimal Debito { get { return this.totDebit; } }

        public decimal Total
        {
            get
            {
                decimal _total = 0;
                if (IsBalanced())
                {
                    //  entries.ForEach(x => total+= x.Credito);
                    _total = entries.Sum(x => x.Credito);
                    return _total;
                }
                throw new InvalidOperationException("El Comprobante no esta balanceado ");
                // return -1; //error
            }
        }

        public bool IsBalanced()
        {
            return balance == decimal.Zero;
        }

        public PartidaDoble GetReverse()
        {
            PartidaDoble revEntry = new PartidaDoble();
            foreach (Anotacion e in this)
            {
                revEntry.Add(e.GetReverse());
            }
            return revEntry;
        }

        #region IList<SingleEntry> Members

        public int IndexOf(Anotacion item)
        {
            return entries.IndexOf(item);
        }

        public void Insert(int index, Anotacion item)
        {
            entries.Insert(index, item);
            AddToTotals(item);
        }

        public void RemoveAt(int index)
        {
            if (index >= 0 && index < entries.Count)
            {
                Anotacion toRemove = entries[index];
                // RemoveFromTotals(toRemove);
                entries.RemoveAt(index);
                UpdateTotals();
            }
            else
            {
                throw new System.ArgumentOutOfRangeException("index");
            }
        }

        public Anotacion this[int index]
        {
            get
            {
                return entries[index];
            }
            set
            {
                if (index >= 0 && index < entries.Count)
                {
                    Anotacion toRemove = entries[index];
                    entries[index] = value;
                    RemoveFromTotals(toRemove);
                    AddToTotals(value);
                }
                else
                {
                    throw new System.ArgumentOutOfRangeException("index");
                }
            }
        }

        private void AddToTotals(Anotacion item)
        {
            UpdateTotals();
            //totDebit = entries.Sum(x => x.Debito);
            //totCredit = entries.Sum(x => x.Credito);

            ////   totDebit += item.Debito;
            ////totCredit += item.Credito;
            //balance = totDebit - totCredit;
        }

        private void UpdateTotals()
        {
            totDebit = entries.Sum(x => x.Debito);
            totCredit = entries.Sum(x => x.Credito);

            //   totDebit += item.Debito;
            //totCredit += item.Credito;
            balance = totDebit - totCredit;
        }

        private void RemoveFromTotals(Anotacion item)
        {
            totDebit =
            //totDebit += item.Debito;
            //totCredit += item.Credito;
            balance = totDebit - totCredit;
        }

        #endregion

        #region ICollection<SingleEntry> Members

        public void Add(Anotacion item)
        {
            entries.Add(item);
            AddToTotals(item);
        }

        public void Clear()
        {
            entries.Clear();
            totDebit = decimal.Zero;
            totCredit = decimal.Zero;
            balance = decimal.Zero;
        }

        public bool Contains(Anotacion item)
        {
            return entries.Contains(item);
        }

        public void CopyTo(Anotacion[] array, int arrayIndex)
        {
            entries.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return entries.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(Anotacion item)
        {
            bool success = entries.Remove(item);
            if (success)
                //RemoveFromTotals(item);
                UpdateTotals();
            return success;
        }

        #endregion

        #region IEnumerable<SingleEntry> Members

        public IEnumerator<Anotacion> GetEnumerator()
        {
            return entries.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
