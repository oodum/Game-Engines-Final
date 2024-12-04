# Game Engine Design and Implementation Final

Adam Tam - 100868600

Note: Most comments are omitted here for brevity. Please refer to the code for more detailed comments.
All scripts are inside `Assets/_Project/_Scripts`

# How to Play
The game is a simple Tetris clone. However, line clears are not implemented.
The controls are as follows:
A / D - Move Left / Right
S - Move Down
J / K - Rotate Left / Right

# Command Pattern

The command pattern implementation I used was for both the moving of the tetrimino as well as the rotation.
Because these are interfaces, adding commands is very easy and doesn't break the rest of the system.
Here are the following interfaces:

```csharp   
public interface ICommand {
    public void Execute(Tetrimino transform);
    public void Reverse(Tetrimino transform);
}
public interface IRotateCommand : ICommand {
    public float RotationAngle { get; }
}
    
public interface IMoveCommand : ICommand {
    public Vector3 Direction { get; }
}
```

These interfaces are implemented for moving left, right, and down, and rotating left and right. 
Here's an example of one of them:
```csharp
public class RotateLeftCommand : IRotateCommand {
    public float RotationAngle => 90;
    public void Execute(Tetrimino tetrimino) {
        tetrimino.transform.Rotate(Vector3.forward, RotationAngle);
    }

    public void Reverse(Tetrimino tetrimino) {
        tetrimino.transform.Rotate(Vector3.forward, -RotationAngle);
    }
}
```

In the player controller, I map the input directly to the command (the following is a snippet of the player controller):
```csharp
public class PlayerController : MonoBehaviour {
    [SerializeField] PlayerInputProcessor input;
    ICommand RotateLeftCommand => new RotateLeftCommand();

    Tetrimino currentTetrimino;
    void Start() {
        input.SetPlayer();
    }

    void Update() {
        currentTetrimino = GameManager.Instance.CurrentTetrimino;
    }

    void OnEnable() {
        input.OnRotateLeftEvent += RotateLeft;
    }

    void OnDisable() {
        input.OnRotateLeftEvent -= RotateLeft;
    }

    void RotateLeft() { currentTetrimino.ExecuteCommand(RotateLeftCommand); }
}
```

... from where the tetrimino can deal with the execution of the command:
```csharp
public void ExecuteCommand(ICommand command) {
    command.Execute(this);
}        
public void ReverseCommand(ICommand command) {
    command.Reverse(this);
}
```

# Object Pooling
The object pooling implementation I used was for the tetriminos. Here's how it works:
```csharp
 public class ObjectPool : MonoBehaviour {
    readonly Dictionary<Type, Queue<Tetrimino>> pool = new();
    [SerializeField] int size;
    public T Get<T>() where T : Tetrimino {
        if (!pool.TryGetValue(typeof(T), out var queue) || queue.Count <= 0) return null;
        var tetrimino = (T)queue.Dequeue();
        tetrimino.gameObject.SetActive(true);
        return tetrimino;
    }

    public void Return<T>(T tetrimino) where T : Tetrimino {
        if (!pool.ContainsKey(typeof(T))) pool.Add(typeof(T), new());
        tetrimino.gameObject.SetActive(false);
        pool[typeof(T)].Enqueue(tetrimino);
    }

    void Start() {
        Initialize<BlueTetrimino>();
        Initialize<RedTetrimino>();
        Initialize<GreenTetrimino>();
        Initialize<YellowTetrimino>();
        Initialize<OrangeTetrimino>();
        Initialize<PurpleTetrimino>();
        Initialize<CyanTetrimino>();
    }

    public void Initialize<T>() where T : Tetrimino {
        pool.Add(typeof(T), new(size));
        for (var i = 0; i < size; i++) {
            var tetrimino = TetriminoFactory.Create<T>();
            Return(tetrimino);
        }
    }
}
```
As you can see, I'm using a dictionary to store the tetriminos, and I'm using a queue to store the tetriminos of the same type.
This is because I cannot just store tetriminos in a queue, as they are of different types. The GameManager should be in charge of picking
the correct Tetrimino, not the ObjectPool. The ObjectPool should only be in charge of storing and returning the tetriminos.

The rest of the script is quite simple. It initializes the pool with the size of the pool, and has its respective Get and Return methods.

Unfortunately, I didn't have the luxury of creating a non-pooled version to test the difference in memory, but in the following profiling,
The memory usage is fairly stable, with max peaks for allocated GC of around 0.5KB, but everything else is completely stable.

![Screenshot From 2024-12-04 14-31-47.png](Screenshot%20From%202024-12-04%2014-31-47.png)

# Other pattern: Factory
I decided to use the factory pattern because the logic is isn't as simple as spawning the tetrimino. You need to know which
type of Tetrimino to spawn, and more importantly, where to spawn it. Instead of cluttering another class with this logic along with the
references to every tetrimino prefab, I decided to this within its own class:
```csharp
public class TetriminoFactory : Singleton<TetriminoFactory> {
    [SerializeField] Tetrimino blueTetrimino, redTetrimino, greenTetrimino, yellowTetrimino, orangeTetrimino, purpleTetrimino, cyanTetrimino;
    [SerializeField] Transform spawnPosition;
    [SerializeField] float scale;
    public static T Create<T>() where T : Tetrimino {
        var tetrimino = Instantiate(GetPrefab<T>(), Instance.spawnPosition.position, Quaternion.identity);
        tetrimino.transform.localScale = Vector3.one*Instance.scale;
        return tetrimino;
    }

    static T GetPrefab<T>() where T : Tetrimino {
        // unfortunately, a switch statement/expression could not be used due to the typeof check not being a constant
        if (typeof(T) == typeof(BlueTetrimino)) return Instance.blueTetrimino as T;
        if (typeof(T) == typeof(RedTetrimino)) return Instance.redTetrimino as T;
        if (typeof(T) == typeof(GreenTetrimino)) return Instance.greenTetrimino as T;
        if (typeof(T) == typeof(YellowTetrimino)) return Instance.yellowTetrimino as T;
        if (typeof(T) == typeof(OrangeTetrimino)) return Instance.orangeTetrimino as T;
        if (typeof(T) == typeof(PurpleTetrimino)) return Instance.purpleTetrimino as T;
        if (typeof(T) == typeof(CyanTetrimino)) return Instance.cyanTetrimino as T;
        throw new ArgumentException("Invalid type");
    }
```

In this class, the factory has a direct reference to the prefabs, as well as the spawn position and scale.
You may have noticed even more generic methods (I love generics). This is because there are different types of Tetriminoes,
but you should be able to use one method to create them instead of having to create a method for each type.
In the Create<T> generic method, it instantiates the prefab at the spawn position and sets the scale. The GetPrefab<T> method is used to get the prefab
based on the type of Tetrimino. This is done by checking the type of T and returning the correct prefab. If the type is invalid, it throws an exception.