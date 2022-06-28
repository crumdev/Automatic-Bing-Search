using System.Collections.Generic;
using Automation.Models;

public interface IMethods
    {
        void CleanUp();
        List<string> GetRandomWords(int numberOfWords);
        void SearchWebInDesktopMode();
        void SearchWebInMobileMode();
        void StartSearchProcess(BingUser user, int searchCount);
    }