   M S B A M L   `   `   `  ���� B  >Program, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:SEEL.LinguisticProcessorSEEL.LinguisticProcessor  2    ,SEEL.LinguisticProcessor.ProjectsStatsWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     8local&clr-namespace:SEEL.LinguisticProcessor     -   $��WordExtractorW��  � Title$  Project Statistics��$��297.378��$��559.648�� � WindowState$ Normal=� � 
ResizeMode$ CanMinimize=�.���  � RowDefinitions �� $��19 ��� $��95 ��� $	��124 ��� $��40 �.+�o� -    o�Name$ Tools_mn��$
��Left=�$��19��$	��Top=�$��41��$��0��.@�m� $��_Tools��.@�m� -   $��_Word Extractor��m� -   $��_Linguistic Change��m� $��_Link Extractor��m� $��_Project Downloader��m� -   $��_Projects Statistics���� m� $��_Exit���� -    ��Name$ Source��$��GitHub statistics��$
��Left=�$��76��$��	10,10,0,0q�$	��Top=�$	��534��$��1��.��� .+��� -    ��Name$ Overwrite_cb��$ ��Overwrite existing dataset��$
��Left=�$��
256,10,0,0q�$	��Top=� d� 	IsChecked$ FalseY��� -    ��Name$ StartCollectingStats_bt��$��Start!��$
��Left=�$��
455,42,0,0q�$	��Top=�$��75��$��20��$��1���� -	   $��	StatusBar��$��Stretch=�$��24��$	��Top=�$��4��.@�� $	��544��	 � ColumnDefinitions	 �� $	��69* ��� $
��457* �.+��� -
   
 ��Name$
 StatusBlock��$��5,0,10,0q�$��1������ *	Message+ �� UpdateSourceTrigger$ PropertyChanged=��� -   $
 Progress������ *
Progress+$ PropertyChanged=��� -   $ Source_Copy��$��Local statistics��$
��Left=�$	��100��$��	10,10,0,0q�$	��Top=�$	��534��$��2��.��� .+��� -   $
 	textBlock��$
��Left=�$��	10,10,0,0q�$
��Wrap=�$8��2Calculates KLOC, # of releases, and their averages��$	��Top=�$	��163���� -   $
 textBlock_Copy��$
��Left=�$��
10,-87,0,0q�$
��Wrap=�$:��4Collects statistics from GitHub (stars, forks, etc.)��$	��Top=�$	��163���� -    ��Name$ FolderProjects��$
��Left=�$��19��$��	90,47,0,0q� �� TextWrapping$
 Wrap=�$��Select language folder...��
�������$	��Top=�$	��272�� �� 
IsReadOnly . {�.  $��0���� -   $ ProjectInputButton��$��	Browse...��$
��Left=�$��	10,47,0,0q�$	��Top=�$��75��$��19��$��0���� -    ��Name$ LanguageSelector��$
��Left=�$��
367,47,0,0q�$	��Top=�$��69���� -   $" StartCollectingLocalStats_bt��$��Start!��$
��Left=�$��
455,74,0,0q�$	��Top=�$��75��$��20��$��2��{�.  