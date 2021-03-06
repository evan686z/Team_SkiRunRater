﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiRunRater
{
    public class Controller
    {
        #region FIELDS

        bool active = true;

        #endregion

        #region PROPERTIES


        #endregion

        #region CONSTRUCTORS

        public Controller()
        {
            ApplicationControl();
        }

        #endregion

        #region METHODS

        private void ApplicationControl()
        {
            SkiRunRepository skiRunRepository = new SkiRunRepository();

            ConsoleView.DisplayWelcomeScreen();

            using (skiRunRepository)
            {
                List<SkiRun> skiRuns = skiRunRepository.GetSkiAllRuns();
                int skiRunID;
                SkiRun skiRun;
                string message;

                while (active)
                {
                    AppEnum.ManagerAction userActionChoice;

                    userActionChoice = ConsoleView.GetUserActionChoice();

                    switch (userActionChoice)
                    {
                        case AppEnum.ManagerAction.None:
                            break;
                        case AppEnum.ManagerAction.ListAllSkiRuns:
                            ConsoleView.DisplayAllSkiRuns(skiRuns);
                            ConsoleView.DisplayContinuePrompt();
                            break;
                        case AppEnum.ManagerAction.DisplaySkiRunDetail:
                            break;
                        case AppEnum.ManagerAction.DeleteSkiRun:
                            //
                            // TODO write a ConsoleView method to get the ski run ID
                            //
                            skiRunID = ConsoleView.GetSkiRunID(skiRuns);
                            skiRunRepository.DeleteSkiRun(skiRunID);
                            ConsoleView.DisplayReset();
                            message = String.Format("Ski Run ID: {0} has been deleted.", skiRunID);
                            ConsoleView.DisplayMessage(message);
                            ConsoleView.DisplayContinuePrompt();
                            ConsoleView.DisplayReset();
                            break;
                        case AppEnum.ManagerAction.AddSkiRun:
                            skiRun = ConsoleView.AddSkiRun();
                            skiRunRepository.InsertSkiRun(skiRun);
                            ConsoleView.DisplayContinuePrompt();
                            break;
                        case AppEnum.ManagerAction.UpdateSkiRun:
                            skiRunID = ConsoleView.GetSkiRunID(skiRuns);
                            skiRun = skiRunRepository.GetSkiRunByID(skiRunID);
                            skiRun = ConsoleView.UpdateSkiRun(skiRun);
                            skiRunRepository.UpdateSkiRun(skiRun);

                            ConsoleView.DisplayReset();
                            message = String.Format("Ski Run: {0} has been updated.", skiRun.Name);
                            ConsoleView.DisplayMessage(message);
                            ConsoleView.DisplayContinuePrompt();
                            ConsoleView.DisplayReset();
                            break;
                        case AppEnum.ManagerAction.QuerySkiRunsByVertical:
                            List<SkiRun> matchingSkiRuns = new List<SkiRun>();

                            int minVertical;
                            int maxVertical;
                            ConsoleView.QueryVerticals(out minVertical, out maxVertical);

                            matchingSkiRuns = skiRunRepository.QueryByVertical(minVertical, maxVertical);

                            ConsoleView.DisplayQueryResults(matchingSkiRuns);
                            ConsoleView.DisplayContinuePrompt();
                            break;
                        case AppEnum.ManagerAction.Quit:
                            active = false;
                            break;
                        default:
                            break;
                    }
                }
            }

            ConsoleView.DisplayExitPrompt();
        }

        #endregion

    }
}
