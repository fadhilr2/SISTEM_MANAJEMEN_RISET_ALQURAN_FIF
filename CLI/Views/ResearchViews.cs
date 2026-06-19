using System;
using Lib.common;
using Lib.models;
using Lib.services;

namespace CLI.Views
{
    public static class ResearchViews
    {
        public static void PrintAllResearch()
        {
            BaseViews.PrintSubheader("ALL RESEARCH");

            foreach (Paper paper in DataContext.Papers.GetAll())
            {
                Console.WriteLine(paper.Title);
                Console.WriteLine($"By: {paper.Author}");
                Console.WriteLine("-----");
            }

            Console.WriteLine();

            ShowAllResearchAction();

            Console.WriteLine();
        }

        public static void OpenResearch(Paper paper)
        {
            BaseViews.PrintSubheader(paper.Title);

            Console.WriteLine($"By: {paper.Author}");
            Console.WriteLine($"Published on: {paper.Date}");

            Console.WriteLine();

            Console.WriteLine("Abstract:");
            Console.WriteLine(paper.Paper_Abstract);

            Console.WriteLine();

            ShowResearchAction(paper);

            Console.WriteLine();
        }

        public static void ShowAllResearchAction()
        {
            Console.WriteLine("1. Open Research");
            Console.WriteLine("0. Back");
            int input = InputReader.ReadInput<int>();

            switch (input)
            {
                case 1:
                    Console.WriteLine(Messages.Get("prompt.enter_title"));
                    string title = InputReader.ReadInput<string>();

                    Console.WriteLine();

                    Paper? paper = PaperService.SearchPaper(title);
                    if (paper == null)
                    {
                        Console.WriteLine(Messages.Get("paper.not_found"));
                        break;
                    }

                    OpenResearch(paper);
                    break;

                case 0:
                    break;

                default:
                    Console.WriteLine("Invalid selection. Please choose 0 or 1.");
                    break;
            }
        }

        public static void ShowResearchAction(Paper paper)
        {
            Console.WriteLine("1. View Researcher's Profile");

            bool canEdit = PaperService.CanEdit(paper);
            if (canEdit)
            {
                Console.WriteLine("2. Edit Title");
                Console.WriteLine("3. Edit Abstract");
                Console.WriteLine("4. Delete");
            }
            Console.WriteLine("0. Back");
            int input = InputReader.ReadInput<int>();

            Console.WriteLine();

            switch (input)
            {
                case 1:
                    OpenResearcherProfile(paper.Author);
                    break;

                case 2:
                    if (!canEdit)
                    {
                        Console.WriteLine(Messages.Get("permission.denied"));
                        break;
                    }

                    Console.WriteLine(Messages.Get("prompt.enter_title"));
                    string newTitle = InputReader.ReadInput<string>();
                    if (PaperService.EditTitle(paper, newTitle))
                    {
                        Console.WriteLine("~Title changed successfully");
                    }
                    else
                    {
                        Console.WriteLine(Messages.Get("permission.denied"));
                    }
                    break;

                case 3:
                    if (!canEdit)
                    {
                        Console.WriteLine(Messages.Get("permission.denied"));
                        break;
                    }

                    Console.WriteLine(Messages.Get("prompt.enter_abstract"));
                    string newAbstract = InputReader.ReadInput<string>();
                    if (PaperService.EditAbstract(paper, newAbstract))
                    {
                        Console.WriteLine("~Abstract changed successfully");
                    }
                    else
                    {
                        Console.WriteLine(Messages.Get("permission.denied"));
                    }
                    break;

                case 4:
                    if (!canEdit)
                    {
                        Console.WriteLine(Messages.Get("permission.denied"));
                        break;
                    }

                    if (PaperService.DeletePaper(paper))
                    {
                        Console.WriteLine(Messages.Get("confirm.deleted"));
                    }
                    else
                    {
                        Console.WriteLine(Messages.Get("permission.denied"));
                    }
                    break;

                case 0:
                    break;

                default:
                    Console.WriteLine(Messages.Get("error.invalid_selection"));
                    break;
            }
        }

        public static void OpenResearcherProfile(string name)
        {
            User? researcher = UserService.SearchUser("name", name);
            string profileName = researcher?.Name ?? name;

            BaseViews.PrintSubheader(profileName);

            foreach (Paper paper in DataContext.Papers.GetAll())
            {
                if (paper.Author.Equals(profileName, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine(paper.Title);
                    Console.WriteLine($"By: {paper.Author}");
                    Console.WriteLine("-----");
                }
            }

            Console.WriteLine();
        }
    }
}
