using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapayaX2.Helpers
{
    public class PaginationHelper
    {
        public int num_pages { get; set; }

        public int num_start_end_link { get; set; }

        public int num_page_link_rendered { get; set; }

        public string url_format { get; set; }

        public string next_delimiter = "<span>{0}</span>";

        public string prev_delimiter = "<span>{0}</span>";

        public string first_delimiter = "<span>{0}</span>";

        public string last_delimiter = "<span>{0}</span>";

        public string selected_delimiter = "<span class=\"current\">{0}</span>";

        public string page_delimiter = "<span>{0}</span>";

        public string skip_delimiter = "<span>{0}</span>";

        public string prev_string = "« Prev";

        public string next_string = "Next >";

        public string first_string = "« First";

        public string last_string = "Last »";

        public string skip_string = "...";

        public PaginationHelper(int num_pages, string url_format, int num_start_end_link = 2, int num_page_link_rendered = 6)
        {
            this.num_pages = num_pages;
            this.url_format = url_format;
            if (num_start_end_link < 2)
            {
                this.num_start_end_link = 2;
            }
            else {
                this.num_start_end_link = num_start_end_link;
            }

            if (num_page_link_rendered < 6)
            {
                this.num_page_link_rendered = 6;
            }
            else {
                this.num_page_link_rendered = num_page_link_rendered;
            }
        }

        public string render_pagination(int page)
        {
            string url_first = string.Empty;
            string url_last = string.Empty;
            string url_next = string.Empty;
            string url_prev = string.Empty;

            string link_first = string.Empty;
            string link_last = string.Empty;
            string link_next = string.Empty;
            string link_prev = string.Empty;

            string link_start = string.Empty;
            string link_end = string.Empty;

            string pagination_first_string = string.Empty;
            string pagination_last_string = string.Empty;

            if (num_pages > 1)
            {
                url_first = String.Format(url_format, 1);
                link_first = "<a href=\"" + url_first + "\">" + first_string + "</a>";
                pagination_first_string = String.Format(first_delimiter, link_first);

                url_last = String.Format(url_format, num_pages);
                link_last = "<a href=\"" + url_last + "\">" + last_string + "</a>";
                pagination_last_string = String.Format(last_delimiter, link_last);
            }

            string pagination_next_string = string.Empty;
            string pagination_prev_string = string.Empty;

            if (page >= 1 && page < num_pages)
            {
                url_next = String.Format(url_format, (int)(page + 1));
                link_next = "<a href=\"" + url_next + "\">" + next_string + "</a>";
                pagination_next_string = String.Format(next_delimiter, link_next);
            }

            if (page <= num_pages && page > 1)
            {
                url_prev = String.Format(url_format, (int)(page - 1));
                link_prev = "<a href=\"" + url_prev + "\">" + prev_string + "</a>";
                pagination_prev_string = String.Format(prev_delimiter, link_prev);
            }

            int num_page_link_rendered_half = Convert.ToInt32(Math.Ceiling((double)num_page_link_rendered / 2));
            int upper_limit = num_pages - num_page_link_rendered;

            int start;
            int end;

            if (page > num_page_link_rendered_half)
            {
                start = Math.Max(Math.Min((page - num_page_link_rendered_half), upper_limit), 1);
                end = Math.Min((page + num_page_link_rendered_half), num_pages);
            }
            else {
                start = 1;
                end = Math.Min(num_page_link_rendered, num_pages);
            }

            string pagination_interval_string = string.Empty;

            //Generate interval link
            if (num_pages > 1)
            {
                for (int i = start; i <= end; i++)
                {
                    string url_page = String.Format(url_format, i);
                    string link_page = "<a href=\"" + url_page + "\">" + i + "</a>";
                    if (i == page)
                    {
                        pagination_interval_string += String.Format(selected_delimiter, link_page);
                    }
                    else
                    {
                        pagination_interval_string += String.Format(page_delimiter, link_page);
                    }
                }
            }

            //Generate pagination begin link
            string pagination_begin_string = string.Empty;
            if (start > 1)
            {
                int begin = start - num_start_end_link;
                if (begin > 1)
                {
                    pagination_interval_string = String.Format(skip_delimiter, skip_string) + pagination_interval_string;
                }

                begin = Math.Min((start - 1), num_start_end_link);
                for (int i = 1; i <= begin; i++)
                {
                    string url_page = String.Format(url_format, i);
                    string link_page = "<a href=\"" + url_page + "\">" + i + "</a>";
                    pagination_begin_string += String.Format(page_delimiter, link_page);
                }
            }

            //Generate pagination finish link
            string pagination_finish_string = string.Empty;
            if (end < num_pages)
            {
                int finish = end + num_start_end_link;
                if (finish < num_pages)
                {
                    pagination_interval_string = pagination_interval_string + String.Format(skip_delimiter, skip_string);
                }

                finish = Math.Max((end + 1), ((num_pages - num_start_end_link) + 1));
                for (int i = finish; i <= num_pages; i++)
                {
                    string url_page = String.Format(url_format, i);
                    string link_page = "<a href=\"" + url_page + "\">" + i + "</a>";
                    pagination_finish_string += String.Format(page_delimiter, link_page);
                }
            }

            //Make the string
            string pagination_all_string = pagination_first_string
                + pagination_prev_string
                + pagination_begin_string
                + pagination_interval_string
                + pagination_finish_string
                + pagination_next_string
                + pagination_last_string;

            //Return the pagination string
            return pagination_all_string;
        }

    }
}