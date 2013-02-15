NBinarySearch
=============

Flexible binary search for all indexed collections in .NET
----------------------------------------------------------

### What is this for?

Out of the box, the .NET framework provides a binary search implementation only for the `List<T>` class and `T[]` arrays. This can be frustrating, as it is sometimes desirable for performance reasons to perform a binary search on an arbitrary `IList<T>` and calling `ToList()` or `ToArray()` for this purpose causes the performance-tuners among us to toss and turn at night.

With **NBinarySearch** you can sleep soundly again. Take a look:

```csharp
// NBinarySearch allows us to perform a binary search on any indexed collection,
// not just List<T> and T[].
IList<string> userNames = GetUserNames();
int johnnyIndex = userNames.BinarySearch("Johnny");
```

### That's it?

Nope. What's also missing from .NET is the ability to binary search a list of elements that are sorted by some *property*. The naïve way to work around this is to use `Select()` followed by `ToList()`; but again, this incurs an unnecessary performance hit by populating an entirely new (potentially large) collection.

```csharp
class Person
{
  public string Name { get; set; }
  public int Age { get; set; }
}

// Search by any property of an object.
IList<Person> users = GetUsers();
int samanthaIndex = users.BinarySearchByKey("Samantha", user => user.Name);

// We can also specify a custom comparer, no problem.
class AgeGroupComparer : IComparer<int>
{
  public int Compare(int x, int y)
  {
    return GetAgeGroup(x).CompareTo(GetAgeGroup(y));
  }

  int GetAgeGroup(int age)
  {
    if (age < 18)
      return 0;
    else if (age < 30)
      return 1;
    else if (age < 45)
      return 2;
    else if (age < 60)
      return 3;
    else
      return 4;
  }
}

var john = new Person { Name = "John", Age = 18 };
IList<Person> activeUsers = GetActiveUsers();
int johnIndex = activeUsers.BinarySearchByValue(john, p => p.Age, new AgeGroupComparer());
```

Hopefully from the brief examples above you get a sense of where **NBinarySearch** is useful.

### Installation

This one's pretty straightforward. First clone the repo:

```
$ git clone git://github.com/dtao/NBinarySearch.git
```

Then build locally with whatever toolchain you prefer, and you should be good to go.

### Authors and Contributors

**NBinarySearch** was developed by [Dan Tao](http://philosopherdeveloper.com) (<a href="https://github.com/dtao" class="user-mention">@dtao</a> on GitHub), an engineer in San Francisco.

I currently work at [Cardpool](http://www.cardpool.com) where, ironically, we don't use .NET at all.

### Contact

Feel free to reach out to me at daniel.tao@gmail.com with any questions or concerns. If you find any bugs in this library or want to add features to it, create a ticket or submit a pull request!
