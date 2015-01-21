FlauLib
=======

A collection of various useful .Net/C# classes and snippets.

### Build Status
|Repo|Appveyor|
|:---|:------------------|
|[FlauLib](https://github.com/Roemer/FlauLib)|[![Build status](https://ci.appveyor.com/api/projects/status/gp0y9qpo4s0nfmrk?svg=true)](https://ci.appveyor.com/project/RomanBaeriswyl/flaulib)|

# Content
### LogitechArx
Various classes to help with using the Logitech Arx SDK in C#
### MVVM
Helpers to use in MVVM in WPF
##### ObservableObject
Base class which encapsulates the INotifyPropertyChanged in an easy way.
Usage:
```csharp
public class SomeClass : ObservableObject {
	public int Age { get { return GetProperty<int>(); } set { SetProperty(value); } }
	public string FirstName {
		get { return GetProperty<string>(); }
		set { SetProperty(value); OnPropertyChanged(() => FullName); }
	}
	public string LastName {
		get { return GetProperty<string>(); }
		set { SetProperty(value); OnPropertyChanged(() => FullName); }
	}
	public string FullName { get { return FirstName + " " + LastName; } }
}
```
##### PropertyChangedProxy
Classo to easily allow to perform actions (like OnPropertyChanged) if a certain property on some other class changed.
Usage:
```csharp
public class MyModel : INotifyPropertyChanged {
    private string _status;
    public string Status {
        get { return _status; }
        set { _status = value; OnPropertyChanged(); }
    }
    // INotifyPropertyChanged implementation ...
}

public class MyViewModel : INotifyPropertyChanged {
    public string Status { get { return _model.Status; } }
    private PropertyChangedProxy<MyModel, string> _statusPropertyChangedProxy;
    private MyModel _model;
    public MyViewModel(MyModel model) {
        _model = model;
        _statusPropertyChangedProxy = new PropertyChangedProxy<MyModel, string>(
            _model, myModel => myModel.Status, s => OnPropertyChanged("Status")
        );
    }
    // INotifyPropertyChanged implementation ...
}
```
##### RelayCommand / TypedRelayCommand
Basic implementation for ICommand. Allows setting the execute method and an optional CanExecute.
The "typed" version additionally allows to get the command parameter as the specifyed type.
Usage:
```csharp
var simpleCommand = new RelayCommand(o => ShowMessageBox("Hello"));
var addNumber = new TypedRelayCommand<int>(o => Add(o), o => o >= 0);
```
### Tools
Random tools for various problems
### WinForms
Controls and classes for WinForms applications
