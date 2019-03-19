using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyLab.Server.DTOs
{
    public class SearchFilter
    {
        private uint size;
        private uint index;
        private const uint MaxPageSize = 1000;
        private string appId;
        private string keyword;
        private string sortBy;

        public SearchFilter()
        {
            PageSize = 10;
            ApplicationId = string.Empty;
            keyword = string.Empty;
            sortBy = string.Empty;
            SortOrder = SortOrders.Default;
        }

        public string ApplicationId
        {
            get { return appId; }
            set { appId = value ?? string.Empty; }
        }

        public string Keyword
        {
            get { return keyword; }
            set { keyword = value ?? string.Empty; }
        }

        public uint PageSize
        {
            get { return size; }
            set
            {
                size = Math.Min(value, MaxPageSize);
                if (size == 0)
                {
                    size = MaxPageSize;
                }
            }
        }

        public uint PageIndex
        {
            get { return index; }
            set
            {
                index = Math.Min(value, int.MaxValue / MaxPageSize);
            }
        }

        public string SortBy
        {
            get { return sortBy; }
            set { sortBy = value ?? string.Empty; }
        }

        public SortOrders SortOrder { get; set; }
    }
}
