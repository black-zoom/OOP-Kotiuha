using System;

class Electronic
{
    private string _brand;
    private double _powerConsumption;

    public string Brand
    {
        get { return _brand; }
        set { _brand = value; }
    }

    public double PowerConsumption
    {
        get { return _powerConsumption; }
        set { _powerConsumption = value; }
    }

    public Electronic(string brand, double powerConsumption)
    {
        _brand = brand;
        _powerConsumption = powerConsumption;
    }

    public virtual void TurnOn()
    {
        Console.WriteLine($"Електронний пристрій {Brand} увімкнено.");
    }

    public string GetElectronicType()
    {
        return "Електронний пристрій";
    }
}

class Television : Electronic
{
    private string _screenResolution;

    public string ScreenResolution
    {
        get { return _screenResolution; }
        set { _screenResolution = value; }
    }

    public Television(string brand, double powerConsumption, string screenResolution)
        : base(brand, powerConsumption)
    {
        _screenResolution = screenResolution;
    }

    public override void TurnOn()
    {
        Console.WriteLine($"Телевізор {Brand} увімкнено. Роздільна здатність: {ScreenResolution}.");
    }

    public void ChangeChannel()
    {
        Console.WriteLine($"Телевізор {Brand}: канал змінено.");
    }

    public new string GetElectronicType()
    {
        return "Телевізор";
    }
}

class Radio : Electronic
{
    private string _frequencyRange;

    public string FrequencyRange
    {
        get { return _frequencyRange; }
        set { _frequencyRange = value; }
    }

    public Radio(string brand, double powerConsumption, string frequencyRange)
        : base(brand, powerConsumption)
    {
        _frequencyRange = frequencyRange;
    }

    public override void TurnOn()
    {
        Console.WriteLine($"Радіо {Brand} увімкнено. Діапазон частот: {FrequencyRange}.");
    }

    public void TuneStation()
    {
        Console.WriteLine($"Радіо {Brand}: станцію налаштовано.");
    }
}

class Program
{
    static void Main()
    {
        Electronic electronic = new Electronic("Sony", 50);
        Television television = new Television("Samsung", 120, "4K");
        Radio radio = new Radio("Philips", 20, "FM 87.5-108 MHz");

        electronic.TurnOn();
        television.TurnOn();
        radio.TurnOn();

        Electronic device1 = television;
        Electronic device2 = radio;

        device1.TurnOn();
        device2.TurnOn();

        television.ChangeChannel();
        radio.TuneStation();

        Console.WriteLine(electronic.GetElectronicType());
        Console.WriteLine(television.GetElectronicType());

        Electronic televisionAsElectronic = television;
        Console.WriteLine(televisionAsElectronic.GetElectronicType());
    }
}

