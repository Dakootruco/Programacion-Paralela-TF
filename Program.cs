using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.WriteLine("Bienvenido a la de carrera de autos.");

        Task<int> car1 = SimulateCarAsync("Auto 1", 1000);
        Task<int> car2 = SimulateCarAsync("Auto 2", 1200);
        Task<int> car3 = SimulateCarAsync("Auto 3", 1500);

       
        Task.WhenAny(car1, car2, car3).ContinueWith(t =>
        {
            Console.WriteLine($"El {GetWinningCar(t.Result.Result)} ganó la carrera!");
        }, TaskContinuationOptions.OnlyOnRanToCompletion);

        Task allCarsFinished = Task.WhenAll(car1, car2, car3);

        await allCarsFinished;

        Console.WriteLine("\n--- Resultados Finales ---");
        Console.WriteLine($"Auto 1 terminó en: {car1.Result} ms.");
        Console.WriteLine($"Auto 2 terminó en: {car2.Result} ms.");
        Console.WriteLine($"Auto 3 terminó en: {car3.Result} ms.");
        Console.WriteLine("Carrera finalizada. ¡Gracias por jugar!");
    }

    
    static async Task<int> SimulateCarAsync(string carName, int maxSpeed)
    {
        Console.WriteLine($"{carName} ha iniciado la carrera.");

       
        int totalTime = 0;

        
        await Task.Factory.StartNew(async () =>
        {
            for (int i = 0; i < 10; i++) // 10 puntos de control
            {
                int delay = new Random().Next(200, maxSpeed); // Tiempo aleatorio en cada punto
                await Task.Delay(delay);
                totalTime += delay;

                //progreso del auto
                Console.WriteLine($"{carName} pasó el punto {i + 1} en {totalTime} ms.");
            }
        }, TaskCreationOptions.AttachedToParent).Unwrap();

        Console.WriteLine($"{carName} terminó la carrera en {totalTime} ms.");
        return totalTime; 
    }

    static string GetWinningCar(int winningTime)
    {
        if (winningTime < 1200) return "Auto 1";
        if (winningTime < 1500) return "Auto 2";
        return "Auto 3";
    }
}
