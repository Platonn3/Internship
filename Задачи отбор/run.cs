using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


class HotelCapacity
{
    static bool CheckCapacity(int maxCapacity, List<Guest> guests)
    {
        var checkInDates = new List<DateTime>();
        var checkOutDates = new List<DateTime>();
        foreach (var guest in guests)
        {
            checkInDates.Add(DateTime.Parse(guest.CheckIn));
            checkOutDates.Add(DateTime.Parse(guest.CheckOut));
        }
        
        checkInDates.Sort();
        checkOutDates.Sort();

        var guestsCount = 0;
        var i = 0;
        var j = 0;

        while (i < checkInDates.Count && j < checkOutDates.Count)
        {
            if (checkInDates[i] < checkOutDates[j])
            {
                guestsCount++;
                if(guestsCount > maxCapacity)
                    return false;
                i++;
            }
            else
            {
                guestsCount--;
                j++;
            }
        }
        return true;
    }


    class Guest
    {
        public string Name { get; set; }
        public string CheckIn { get; set; }
        public string CheckOut { get; set; }
    }


    static void Main()
    {
        int maxCapacity = int.Parse(Console.ReadLine());
        int n = int.Parse(Console.ReadLine());


        List<Guest> guests = new List<Guest>();


        for (int i = 0; i < n; i++)
        {
            string line = Console.ReadLine();
            Guest guest = ParseGuest(line);
            guests.Add(guest);
        }


        bool result = CheckCapacity(maxCapacity, guests);


        Console.WriteLine(result ? "True" : "False");
    }


    // Простой парсер JSON-строки для объекта Guest
    static Guest ParseGuest(string json)
    {
        var guest = new Guest();


        // Извлекаем имя
        Match nameMatch = Regex.Match(json, "\"name\"\\s*:\\s*\"([^\"]+)\"");
        if (nameMatch.Success)
            guest.Name = nameMatch.Groups[1].Value;


        // Извлекаем дату заезда
        Match checkInMatch = Regex.Match(json, "\"check-in\"\\s*:\\s*\"([^\"]+)\"");
        if (checkInMatch.Success)
            guest.CheckIn = checkInMatch.Groups[1].Value;


        // Извлекаем дату выезда
        Match checkOutMatch = Regex.Match(json, "\"check-out\"\\s*:\\s*\"([^\"]+)\"");
        if (checkOutMatch.Success)
            guest.CheckOut = checkOutMatch.Groups[1].Value;


        return guest;
    }
}