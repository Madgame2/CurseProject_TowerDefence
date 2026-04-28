using System;
using UnityEngine;
using Zenject;

class InputInitializer : IInitializable, IDisposable
{
    private readonly BaseInputActions _input;

    public InputInitializer(BaseInputActions input)
    {
        _input = input;
    }

    public void Initialize()
    {
        _input.Enable();
    }

    public void Dispose()
    {
        _input.Disable();
    }
}
