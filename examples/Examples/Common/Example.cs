using CommandLine;

namespace Concordium.Sdk.Examples.Common;

/// <summary>
/// Represents an example program which supports the command-line
/// parameters defined by instances of <see cref="ExampleOptions"/>.
/// </summary>
public static class Example
{
    /// <summary>
    /// Run an example program specified by a callback and the raw command-
    /// line parameters. The raw command line parameters are parsed according
    /// to the supplied <typeparam name="T"/> and the callback is invoked with
    /// the resulting instance as its argument.
    /// </summary>
    /// <typeparam name="T">
    /// The <see cref="ExampleOptions"/> instance into which <paramref name="args"/>
    /// will be parsed.
    /// </typeparam>
    /// <param name="args">
    /// The raw command line arguments.
    /// </param>
    /// <param name="exampleCallback">
    /// The callback corresponding to the example program which will be
    /// invoked with the parsed <typeparam name="T"/> instance as its
    /// argument.
    /// </param>
    public static Task Run<T>(string[] args, Action<T> exampleCallback)
        where T : ExampleOptions
    {
        Task exampleCallbackTask(T options) =>
            Task.Run(() => exampleCallback(options));

        return Run<T>(args, exampleCallbackTask);
    }

    /// <summary>
    /// Run an asynchronous example program specified by a callback and the
    /// raw command-line parameters. The raw command line parameters are parsed
    /// according to the supplied <typeparam name="T"/> and the callback is invoked
    /// with the resulting instance as its argument.
    /// </summary>
    /// <typeparam name="T">
    /// The <see cref="ExampleOptions"/> instance into which <paramref name="args"/>
    /// will be parsed.
    /// </typeparam>
    /// <param name="args">
    /// The raw command line arguments.
    /// </param>
    /// <param name="exampleCallback">
    /// The asynchronous callback corresponding to the example program which will be
    /// invoked with the parsed <typeparam name="T"/> instance as its argument.
    /// </param>
    public static Task Run<T>(string[] args, Func<T, Task> exampleCallback)
        where T : ExampleOptions => Task.Run(() =>
                                         {
                                             try
                                             {
                                                 Parser.Default
                                                     .ParseArguments<T>(args)
                                                     .WithParsedAsync(options => exampleCallback(options))
                                                     .Wait();
                                             }
                                             catch (Exception e)
                                             {
                                                 HandleCallbackException(e);
                                             }
                                         });


    private static void HandleCallbackException(Exception e)
    {
        Console.WriteLine($"An error occurred while running the example: {e.Message}");
        Environment.Exit(1);
    }
}
