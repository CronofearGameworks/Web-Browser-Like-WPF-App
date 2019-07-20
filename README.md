# Web-Browser-Like-WPF-App
A WPF that resembles a web browser. This is a sample and it's meant to be used as an example of how to mix these libraries, good practices aren't implemented so beware!

It uses Prism.DryIoc, Dragablz, MaterialDesignInXaml and MahApps.Metro Window.

Remember to restore nuget packages before using it!

# What you'll find:

This example is based on: https://github.com/svantreeck/DragablzPrism so i reccomend checking that before!

- MainWidnow.Xaml/.cs:
  - Mix metro Window with Material design.
  - Some styling of the Dragablz Material Design style, using the official style from MaterialDesignInXaml as a starting point. Here the most important change is that i added a command and a event to the ((+) add new tab) button, so ican do some logic when the button is pressed in both the viewmodel and code-behind. I did it because i wasn't able to use Dragablz NewItemFactory, so you may not need this change if you know how.
  - Using a CustomTabablzControl, so i can bind the command previously mentioned.
  
- Added an IActiveAware interface which is hooked to the viewmodels in TabablzRegionBehavior.cs. VM that implement this interface will know then they gain and loss focus when they're part of a TabablzControl region. Useful to solve a bug with Dialog Modals in MaterialDesign while using Dragablz.

Other changes are straightforward and should be easy to understand. Happy Coding!


