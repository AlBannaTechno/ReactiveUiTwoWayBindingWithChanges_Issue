### Description
this issue occur, if you used two way binding on interface that not inherit from **IReactiveObject**

everything will works fine, until you listen to these changes with any listener eg, **WhenPropertyChanged**

you will get heap allocation issue due to infinite loop notifications,

so at this stage you must make your Model interface inherit from **IReactiveObject** 