   M S B A M L   `   `   `  ���� B  >Program, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:SEEL.LinguisticProcessorSEEL.LinguisticProcessor  2    ,SEEL.LinguisticProcessor.WordExtractorWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     8local&clr-namespace:SEEL.LinguisticProcessor     -   $��WordExtractorW��  � Title$  Word Extractor��$	��710��$��559.648�� � WindowState$ Normal=� � 
ResizeMode$ CanMinimize=�.���  � RowDefinitions �� $��19 ��� $	��157 ��� $	��205 ��� $	��268 ��� $��61 �.+�o� -    o�Name$ Tools_mn��$
��Left=�$��19��$	��Top=�$��41��$��0��.@�m� $��_Tools��.@�m� -   $��_Word Extractor��m� -   $��_Linguistic Change��m� $��_Link Extractor��m� -   $��_Project Downloader��m� -   $��_Projects Statistics���� m� $��_Exit���� -    ��Name$ Source��$��Project��$
��Left=�$	��141��$��	10,10,0,0q�$	��Top=�$	��534��$��1��.��� $��	0,0,0,-12q� �� $��55 ��� $��32 ��� $��35 �.+��� -    ��Name$ ProjectInputButton��$��	Browse...��$
��Left=�$��	10,26,0,0q�$	��Top=�$��75��$��20��$��0���� -	    ��Name$ ProjectPath��$
��Left=�$��21��$��
111,25,0,0q� �� TextWrapping$
 Wrap=�$��Select project...��
�������$	��Top=�$	��401��	 �� 
IsReadOnly	 . {�.  $��0���� -
   
 ��Name$
 	textBlock��$
��Left=�$��10,5,0,0q�$
��Wrap=�$"��1. Select the project folder��$	��Top=�$	��480���� -   $ ProcessWholeFolder_bt��$��Process several projects��$
��Left=�$��10,5,0,0q�$	��Top=�$	��135��$��21��$��2���� -   $ StartWordExtraction_bt��$��Start!��$
��Left=�$��358,142,0,0q�$	��Top=�$��75��$��23��$��2��{�.  �� -   $
 
textBlock1��$
��Left=�$��
233,11,0,0q�$
��Wrap=�$	��Top=�$	��311��$��2��$	��113���� $���2. Select the options you want to apply and hit Start!. By default, all options should be selected. Calculate statistics will output the statistics for the project and releases once calculation is finished. ���� �� $7�1"Combine code and comments" merges words together���� -   $ Code_options��$��Code extraction options��$
��Left=�$	��100��$��	10,11,0,0q�$��2��$	��Top=�$	��211��.���  �� $��26 ��� $��26 ��� $��26 ��� $��26 ��� $��26 �.+��� -    ��Name$ IsCalculateStatisticsBox��$��Calculate statistics��$
��Left=�$��Center=�$��10,6,0,4q�$��0��$	��116��$��16�� d� 	IsChecked$
 TrueY��� -   $ IsCamelCaseBox��$��camelCase splitting��$
��Left=�$��Center=�$��10,6,0,4q�$��1��$	��160��$��16��$
 TrueY��� -   $ IsUnderscoreBox��$��Underscore__splitting��$
��Left=�$��Center=�$��10,6,0,4q�$��2��$	��160��$��16��$
 TrueY��� -    ��Name$ 	OutputTab��$��Stretch=�$��Stretch=�$��3��$��0,0,0,1q��� -   $��	StatusBar��$��Stretch=�$��24��$	��Top=�$��4��.@�� $	��544�� � ColumnDefinitions �� $	��69* ��� $
��457* �.+��� -   $
 StatusBlock��$��5,-12,10,-22q�$��2������ *	Message+ �� UpdateSourceTrigger$ PropertyChanged=��� -   $��StatisticsBar��$��Stretch=�$��32��$	��Top=�$��4��.@�� $	��526�� �� $	��85* ��� $
��178* �.+��� $��Files:��$��0��$
��Left=�$��26���� -   $��
FilesTotal��$��31,0,0,0q�$��0��$
��Left=�$��70������ *
FilesTotal+$ PropertyChanged=��� $
��LOC:��$��0,0,40,0q�$��0��$��Right=�$��24���� -   $��TotalLOC��$��	0,0,313,0q�$��0��$��Right=�$��79��$��2������ *LinesOfCode+$ PropertyChanged=��� 	 �� X1$ 0��	 �� X2$ 0��	 �� Y1$ 0��	 �� Y2$ 20��
�������$��1��$��
48,0,271,0q�$��1���� $��Count:��$��
56,0,257,0q�$��1���� -   $��NumberOfWords��$��104,0,183,0q�$��1������ *NumberOfWords+$ PropertyChanged=��� -    ��Name$ Releases_cb��$
��Left=�$��	439,0,0,0q�$��3��$	��Top=�$	��105��$
��Auto=�$��22��