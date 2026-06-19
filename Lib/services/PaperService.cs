using Lib.common;
using Lib.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.services
{
    public static class PaperService
    {
        public static Paper? SearchPaper(string title)
        {
            return DataContext.Papers.Find(paper => title.Equals(paper.Title, StringComparison.OrdinalIgnoreCase));
        }

        public static bool UploadPaper(string title, string paperAbstract)
        {
            if (Session.Instance.Account == null)
            {
                return false;
            }

            DataContext.Papers.Add(new Paper
            {
                Title = title,
                Paper_Abstract = paperAbstract,
                Author = Session.Instance.Account.Name,
                Date = $"{DateTime.Now.Month} {DateTime.Now.Date} {DateTime.Now.Year}"
            });

            return true;
        }

        public static bool CanEdit(Paper paper)
        {
            return Session.Instance.Account != null && (Session.Instance.Account.Name.Equals(paper.Author, StringComparison.OrdinalIgnoreCase) || Session.Instance.Account.Role.Equals("admin", StringComparison.OrdinalIgnoreCase));
        }

        public static bool EditTitle(Paper paper, string newTitle)
        {
            if (!CanEdit(paper))
            {
                return false;
            }

            paper.Title = newTitle;
            return true;
        }

        public static bool EditAbstract(Paper paper, string newAbstract)
        {
            if (!CanEdit(paper))
            {
                return false;
            }

            paper.Paper_Abstract = newAbstract;
            return true;
        }

        public static bool DeletePaper(Paper paper)
        {
            if (!CanEdit(paper))
            {
                return false;
            }

            return DataContext.Papers.Remove(paper);
        }
    }
}
