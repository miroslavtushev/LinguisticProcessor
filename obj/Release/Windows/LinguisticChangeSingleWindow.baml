   M S B A M L   `   `   `  ���� R  NOxyPlot.Wpf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=75e952ba404cdbb0>.clr-namespace:OxyPlot.Wpf;assembly=OxyPlot.WpfOxyPlot.Wpf  B >Program, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:SEEL.LinguisticProcessorSEEL.LinguisticProcessor ;   5SEEL.LinguisticProcessor.LinguisticChangeSingleWindow   6oxy.clr-namespace:OxyPlot.Wpf;assembly=OxyPlot.Wpf  R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     8local&clr-namespace:SEEL.LinguisticProcessor     -   $��LingSingleW��  � Title$  Linguistic Change��$��643.468��$��1124.968�� � WindowState$ Normal=� � 
ResizeMode$ CanMinimize=�.���  � RowDefinitions �� $	��155 ��� $	��428 ��� $	��288 � � ColumnDefinitions �� $	��549 ��� $	��570 �.+�o� -    o�Name$ Tools_mn��$
��Left=�$��19��$	��Top=�$��41��$��0��.@�m� $��_Tools��.@�m� -   $��_Word Extractor��m� -   $��_Linguistic Change��m� $��_Link Extractor��m� $��_Project Downloader��m� -   $��_Projects Statistics���� m� $��_Exit���� -    ��Name$ Source��$��Word Utility for Project��$
��Left=�$	��116��$��	10,29,0,0q�$	��Top=�$	��534��$��0��.��� $��	0,0,0,-12q� �� $��37 ��� .+��� -    ��Name$ ProjectInputButton��$��	Browse...��$
��Left=�$��	10,10,0,0q�$	��Top=�$��75��$��19��$��0���� -    ��Name$ ProjectPath��$
��Left=�$��19��$��	111,9,0,0q�	 �� TextWrapping$
	 Wrap=�$��Select project...��
�������$	��Top=�$	��401��
 �� 
IsReadOnly
 . {�.  $��0���� -	   $ ProcessWholeFolder_bt��$��Process several projects��$
��Left=�$��
377,10,0,0q�$	��Top=�$	��135��$��19��$��1���� -
   $ CalculateWordUtility��$��Calculate word utility��$��19��$
��Left=�$��
111,39,0,0q�$��1��$	��Top=�$	��126��{�.  �� -   $ OutputWordUtilityBt��$
��Show��$��19��$
��Left=�$��	10,39,0,0q�$��1��$	��Top=�$��75��{�.  �� -   $ Output��$��Output��$
��Left=�$	��399��$��	10,10,0,0q�$��1��$	��Top=�$	��534��.���  �� $	��248 ��� $	��286 �.+�&   System.Windows.Controls.DataGrid  -     Name$ 
OutputCode��   AutoGenerateColumns .    CanUserAddRows .  $
��Left=�$	��299��$	��Top=�$	��249��$��0��   
IsReadOnly . $��2��$��0,33,0,0q�   SelectionMode$ Extended=����� *ListOfCodeWords+ �� UpdateSourceTrigger$ PropertyChanged=�  -   $ OutputComments�� .  .  $
��Left=�$	��299��$	��Top=�$	��274��$��1�� . $��0,33,0,0q����� *ListOfCommentWords+$ PropertyChanged=��� -    ��Name$ Releases_cb��$
��Left=�$��	159,0,0,0q�$	��Top=�$	��105��$
��Auto=�$��22��$��1���� -   $ PlotWord��$��Plot selected word��$��19��$
��Left=�$��
10,348,0,0q�$	��Top=�$	��110��{�.  �� -   $ CancelSelection��$��Cancel selection��$��19��$
��Left=�$��128,348,0,0q�$	��Top=�$	��110���� -   $ WordUtilityEvolution��$��Word utility evolution��$
��Left=�$	��257��$��	10,10,0,0q�$��1��$	��Top=�$	��550��$��2��.���  �� $	��205 ��� $��30 �.+�   OxyPlot.Wpf.PlotView  $��0��$��0,0,0,0q�{�.    Model �� *Model+$ PropertyChanged=��� -   $ ClearPlotBt��$��Clear��$
��Left=�$	��Top=�$��75��$��19��$��1�� J� RenderTransformOrigin$ 0.467,-0.737&�$��10,5,0,0q��� -   $��	StatusBar��$��Stretch=�$��24��$	��Top=�$��2��$��2��$��	0,0,-21,0q�.@�� $	��544�� �� $	��69* ��� $
��457* �.+��� -    ��Name$ StatusBlock��$��
5,0,-558,0q�$��1������ *Status+$ PropertyChanged=��� -   $ Progress������ *
Progress+$ PropertyChanged=��� -   $ AdditionalOptions_Copy��$��Additional options��$
��Left=�$	��140��$��
10,152,0,0q�$��1��$	��Top=�$	��550��$��1��.���  �� $��43 ��� $��43 ��� $	��156 ��� $��30 �.+��� -   $) #ProjectInputButtonAdditionalOptions��$��	Browse...��$
��Left=�$��	10,10,0,0q�$	��Top=�$��75��$��19��$��0���� -   $" ProjectPathAdditionalOptions��$
��Left=�$��19��$��	111,9,0,0q�$
	 Wrap=�$��Select project...��
�������$	��Top=�$	��401��
 . {�.  $��0���� -   $  CalculateAdditionalOptions��$��
Calculate!��$��19��$
��Left=�$��
437,10,0,0q�$��1��$	��Top=�$��75��{�.  �� -   $ PlotAdditionalOptions��$
��Plot��$��19��$
��Left=�$��	437,3,0,0q�$��2��$	��Top=�$��75��{�.  	� -    	�Name$ LingChangeRadio��$��Linguistic Change��$
��Left=�$��11,0,0,0q�$��1��$	��Top=� 	� 	GroupName$
 Mode�� d� 	IsChecked$
 TrueY�	� -   $ WordBirthDeathRadio��$��Word Birth and Death��$
��Left=�$��	11,20,0,0q�$��1��$	��Top=�$
 Mode���� -    ��Name$ UniqueCommiters��$ ��Calculate Unique Commiters��$
��Left=�$��	179,0,0,0q�$��1��$	��Top=�$ FalseY��� -   $ BetweenFirstAndLast��$/��)Calculate Between First and Last Releases��$
��Left=�$��
179,20,0,0q�$��1��$	��Top=�$ FalseY�