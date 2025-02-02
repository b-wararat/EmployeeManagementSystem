namespace Client.ApplicationState
{
    public class AllState
    {
        public Action? Action { get; set; }
        public bool ShowGneralDepartment { get; set; }
        public bool ShowDepartment { get; set; }
        public bool ShowBranch { get; set; }
        public bool ShowCountry { get; set; }
        public bool ShowCity { get; set; }
        public bool ShowTown { get; set; }
        public bool ShowEmployee { get; set; }
        public bool ShowUser { get; set; }

        public void ClickedGneralDepartment()
        {
            ResetAll();
            ShowGneralDepartment = true;
            Action?.Invoke();
        }
        public void ClickedDepartment()
        {
            ResetAll();
            ShowDepartment = true;
            Action?.Invoke();
        }

        public void ClickedBranch()
        {
            ResetAll();
            ShowBranch = true;
            Action?.Invoke();
        }

        public void ClickedCountry()
        {
            ResetAll();
            ShowCountry = true;
            Action?.Invoke();
        }

        public void ClickedCity()
        {
            ResetAll();
            ShowCity = true;
            Action?.Invoke();
        }

        public void ClickedTown()
        {
            ResetAll();
            ShowTown = true;
            Action?.Invoke();
        }

        public void ClickedEmployee()
        {
            ResetAll();
            ShowEmployee = true;
            Action?.Invoke();
        }

        public void ClickedUser()
        {
            ResetAll();
            ShowUser = true;
            Action?.Invoke();
        }

        private void ResetAll()
        {
            ShowGneralDepartment = false;
            ShowDepartment = false;
            ShowBranch = false;
            ShowCountry = false;
            ShowCity = false;
            ShowTown = false;
            ShowEmployee = false;
            ShowUser = false;
        }

    }
}
