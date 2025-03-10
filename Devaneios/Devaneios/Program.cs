var task1 = Task.Delay(3000);
var task2 = Task.Delay(4000);
var task3 = Task.Delay(5000);
var task4 = Task.Delay(6000);
var taskError = ErrorAsync();

var taskResult = Task.WhenAll(task1, task2, taskError, task3, task4);

try
{
    await taskResult;
}
catch (Exception e)
{
    Console.WriteLine(e);
}

Console.WriteLine(taskResult.Status);

return;

async Task ErrorAsync()
{
    await Task.Delay(4500);
    //throw new OperationCanceledException();
}