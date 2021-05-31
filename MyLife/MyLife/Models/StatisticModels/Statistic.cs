namespace MyLife.Models.StatisticModels
{
    public class Statistic
    {
        public int AllCompletedTasks { get; set; }
        public int TasksCompletedFrequency { get; set; }
        public float AverageCompletedTasksInDay { get; set; }
        public int CurrentCompletedDaysInRow { get; set; }
        public int BestCompletedDaysInRow { get; set; }
        public int AllCompletedDays { get; set; }
    }
}
