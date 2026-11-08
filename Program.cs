using System;
using System.Collections.Generic;
using System.Linq;

public class Employee
{
    public int Id { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Department { get; set; } = "";
    public string JobTitle { get; set; } = "";
    public decimal MonthlySalary { get; set; }
    public double TaxRate { get; set; }
    public DateTime HireDate { get; set; }
    public bool IsActive { get; set; }
    public string Email { get; set; } = "";
}

class Program
{
    static List<Employee> employees = new List<Employee>();   // starts empty
    static int nextId = 1001;

    static void Main()
    {
        bool running = true;
        while (running)
        {
            Console.Clear();
            Console.WriteLine("\n=== PAYROLL SYSTEM (Feature) ===");
            Console.WriteLine("1. Create");
            Console.WriteLine("2. Read (View All)");
            Console.WriteLine("3. Search / Filter");
            Console.WriteLine("4. Update");
            Console.WriteLine("5. Delete");
            Console.WriteLine("6. Exit");
            string choice = Ask("Select an option: ");
            Console.WriteLine();

            switch (choice)
            {
                case "1": CreateEmployee(); break;
                case "2": ReadEmployees(); break;
                case "3": SearchEmployees(); break;
                case "4": UpdateEmployee(); break;
                case "5": DeleteEmployee(); break;
                case "6": running = false; break;
                default: Console.WriteLine("Invalid choice."); break;
            }

            if (running)
            {
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
            }
        }
    }

    // Prints a label and returns what the user typed (never null)
    static string Ask(string label)
    {
        Console.Write(label);
        return Console.ReadLine() ?? "";
    }

    static void CreateEmployee()
    {
        Console.WriteLine("--- Add New Employee ---");
        string firstName = Ask("First Name: ");
        string lastName = Ask("Last Name: ");
        string department = Ask("Department (e.g., IT, HR, Sales, Finance): ");
        string jobTitle = Ask("Job Title: ");
        decimal.TryParse(Ask("Monthly Salary: "), out decimal salary);
        double.TryParse(Ask("Tax Rate (e.g., 0.12 for 12%): "), out double taxRate);
        DateTime.TryParse(Ask("Hire Date (yyyy-MM-dd): "), out DateTime hireDate);
        bool isActive = Ask("Is Active? (y/n): ").Trim().ToLower() == "y";
        string email = Ask("Email: ");

        employees.Add(new Employee
        {
            Id = nextId++,
            FirstName = firstName,
            LastName = lastName,
            Department = department,
            JobTitle = jobTitle,
            MonthlySalary = salary,
            TaxRate = taxRate,
            HireDate = hireDate,
            IsActive = isActive,
            Email = email
        });

        Console.WriteLine("\nEmployee added successfully!");
    }

    static void ReadEmployees()
    {
        Console.WriteLine("--- Employee List ---");
        PrintTable(employees);
    }

    static void PrintTable(List<Employee> list)
{
    if (list.Count == 0)
    {
        Console.WriteLine("No employees found.");
        return;
    }

    Console.WriteLine("{0,-5} {1,-10} {2,-10} {3,-9} {4,-18} {5,11} {6,5} {7,-10} {8,-6} {9}",
        "Id", "First", "Last", "Dept", "Job Title", "Salary", "Tax", "Hired", "Active", "Email");
    Console.WriteLine(new string('-', 100));

    foreach (var e in list)
    {
        string active = e.IsActive ? "Yes" : "No";
        string tax = $"{e.TaxRate * 100:0.#}%";
        Console.WriteLine($"{e.Id,-5} {e.FirstName,-10} {e.LastName,-10} {e.Department,-9} {e.JobTitle,-18} {e.MonthlySalary,11:N2} {tax,5} {e.HireDate,-10:yyyy-MM-dd} {active,-6} {e.Email}");
    }
}

    static void SearchEmployees()
    {
        Console.WriteLine("--- Search / Filter ---");
        Console.WriteLine("1. By name");
        Console.WriteLine("2. By department");
        Console.WriteLine("3. By status (active/inactive)");
        string choice = Ask("Select: ");

        List<Employee> results;
        switch (choice)
        {
            case "1":
                string name = Ask("Enter name: ");
                results = employees.Where(e =>
                    e.FirstName.Contains(name, StringComparison.OrdinalIgnoreCase) ||
                    e.LastName.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
                break;
            case "2":
                string dept = Ask("Enter department: ");
                results = employees.Where(e =>
                    e.Department.Equals(dept, StringComparison.OrdinalIgnoreCase)).ToList();
                break;
            case "3":
                bool active = Ask("Active? (y/n): ").Trim().ToLower() == "y";
                results = employees.Where(e => e.IsActive == active).ToList();
                break;
            default:
                Console.WriteLine("Invalid choice.");
                return;
        }

        Console.WriteLine($"\n{results.Count} result(s) found.");
        PrintTable(results);
    }

    static void UpdateEmployee()
    {
        ReadEmployees();
        if (employees.Count == 0) return;

        if (!int.TryParse(Ask("\nEnter the ID of the employee to update: "), out int id))
        {
            Console.WriteLine("\nInvalid ID format.");
            return;
        }

        Employee? emp = employees.FirstOrDefault(e => e.Id == id);
        if (emp == null)
        {
            Console.WriteLine("\nEmployee not found.");
            return;
        }

        Console.WriteLine("(Press Enter to keep the current value)");
        string v;

        v = Ask($"First Name ({emp.FirstName}): ");
        if (!string.IsNullOrWhiteSpace(v)) emp.FirstName = v;

        v = Ask($"Last Name ({emp.LastName}): ");
        if (!string.IsNullOrWhiteSpace(v)) emp.LastName = v;

        v = Ask($"Department ({emp.Department}): ");
        if (!string.IsNullOrWhiteSpace(v)) emp.Department = v;

        v = Ask($"Job Title ({emp.JobTitle}): ");
        if (!string.IsNullOrWhiteSpace(v)) emp.JobTitle = v;

        v = Ask($"Monthly Salary ({emp.MonthlySalary}): ");
        if (decimal.TryParse(v, out decimal newSalary)) emp.MonthlySalary = newSalary;

        v = Ask($"Tax Rate ({emp.TaxRate}): ");
        if (double.TryParse(v, out double newTax)) emp.TaxRate = newTax;

        v = Ask($"Hire Date ({emp.HireDate:yyyy-MM-dd}): ");
        if (DateTime.TryParse(v, out DateTime newDate)) emp.HireDate = newDate;

        string current = emp.IsActive ? "y" : "n";
        v = Ask($"Is Active? y/n ({current}): ").Trim().ToLower();
        if (v == "y") emp.IsActive = true;
        else if (v == "n") emp.IsActive = false;

        v = Ask($"Email ({emp.Email}): ");
        if (!string.IsNullOrWhiteSpace(v)) emp.Email = v;

        Console.WriteLine("\nEmployee updated successfully!");
    }

    static void DeleteEmployee()
    {
        ReadEmployees();
        if (employees.Count == 0) return;

        if (!int.TryParse(Ask("\nEnter the ID of the employee to delete: "), out int id))
        {
            Console.WriteLine("\nInvalid ID format.");
            return;
        }

        int removed = employees.RemoveAll(e => e.Id == id);
        Console.WriteLine(removed > 0 ? "\nEmployee deleted successfully!" : "\nEmployee not found.");
    }
}