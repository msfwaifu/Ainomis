namespace Ainomis.Shared.Input.Keyboard {
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Input;

  public class KeyboardBindingSystem : IKeyBindingSystem {
    // Private fields
    private readonly Dictionary<Keys, TimeSpan> keyHeldTimes;
    private KeyboardState currentState;

    public KeyboardBindingSystem() {
      keyHeldTimes = new Dictionary<Keys, TimeSpan>();
      currentState = Keyboard.GetState();

      foreach(Keys key in Enum.GetValues(typeof(Keys))) {
        // Initially all keys have been held for zero seconds
        keyHeldTimes.Add(key, TimeSpan.Zero);
      }
    }

    public void Update(GameTime gameTime) {
      currentState = Keyboard.GetState();

      foreach(Keys key in Enum.GetValues(typeof(Keys))) {
        // Update the current hold time for each key
        if(currentState[key] == KeyState.Down) {
          keyHeldTimes[key] += gameTime.ElapsedGameTime;
        } else {
          keyHeldTimes[key] = TimeSpan.Zero;
        }
      }
    }

    public bool IsBindingActive(IKeyBinding binding) {
      var keyboardBinding = (KeyboardBinding)binding;

      // Ensure the selected key and all its modifiers are active. If no
      // modifiers are specified, the array will be empty, and 'All' will return
      // true. See: https://msdn.microsoft.com/library/bb548541(v=vs.100).aspx
      if(currentState.IsKeyDown(keyboardBinding.Key) && keyboardBinding.Modifiers.All(currentState.IsKeyDown)) {
        if(keyHeldTimes[keyboardBinding.Key] >= keyboardBinding.Duration) {
          return keyboardBinding.Timeout.HasValue ? (keyHeldTimes[keyboardBinding.Key] < keyboardBinding.Timeout.Value) : true;
        }
      }

      return false;
    }

    public Type GetBindingType() => typeof(KeyboardBinding);
  }
}
