# Building Xamarin.Forms from Source

In going beyond building simple Xamarin.Forms apps, you eventually will want to be able to break and step through Xamarin.Forms source code.  This means you'll need to include Xamarin.Forms source code in your project.  This documents how to do this for iOS, Andriod and UWP Xamarin.Forms apps by breaking the process down into:

 - "Git"ting the source


## "Git"ting the source

Because you're wanting to integrate Xamarin.Forms source along side your own source code, your best bet is to add Xamarin.Forms as a Git Submodule of your project.  Git submodules are a great way to maintain version control on separate projects while still simulaniously working with the source code in one solution.  Unfortunately, VisualStudio and Git Submodules are not all pancakes and blueberries so keep reading for some ideas on how to work around the quirks.  

If you don't know how to add Git Submodules, I would first recommend [reading this excellent writeup by Lars Vogel](http://www.vogella.com/tutorials/GitSubmodules/article.html) and then take a moment to Google how to add a submodule for the Git client you use.  Because I use [Atlassian's SourceTree](https://www.sourcetreeapp.com/) I'll demonstr  


