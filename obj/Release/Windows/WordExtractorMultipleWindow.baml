   M S B A M L   `   `   `  ���� B  >Program, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:SEEL.LinguisticProcessorSEEL.LinguisticProcessor  :    4SEEL.LinguisticProcessor.WordExtractorMultipleWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     8local&clr-namespace:SEEL.LinguisticProcessor     -   $��WordExtractorW��  � Title$*  $Word Extractor for Multiple Projects��$��314.886��$��559.648�� � WindowState$ Normal=� � 
ResizeMode$ CanMinimize=�.���  � RowDefinitions �� $��19 ��� $��95 ��� $	��140 ��� $	��120 �.+�o� -    o�Name$ Tools_mn��$
��Left=�$��19��$	��Top=�$��41��$��0��.@�m� $��_Tools��.@�m� -   $��_Word Extractor��m� -   $��_Linguistic Change��m� $��_Link Extractor��m� $��_Project Downloader��m� -   $��_Projects Statistics���� m� $��_Exit���� -    ��Name$ Source��$��Multiple projects��$
��Left=�$��76��$��	10,10,0,0q�$	��Top=�$	��534��$��1��.��� .+��� -    ��Name$ ProjectInputButton��$��	Browse...��$
��Left=�$��	10,26,0,0q�$	��Top=�$��75��$��19���� -    ��Name$ FolderProjects��$
��Left=�$��20��$��
111,25,0,0q� �� TextWrapping$
 Wrap=�$��Select folder(s)...��
�������$	��Top=�$	��401��	 �� 
IsReadOnly	 . {�.  �� -	   
 ��Name$
 	textBlock��$
��Left=�$��10,5,0,0q�$
��Wrap=�$(��"Select the folders of the projects��$	��Top=�$	��480���� -
   $ StartWordExtraction_bt��$��Start!��$
��Left=�$��
345,79,0,0q�$	��Top=�$��75��$��20��$��2��{�.  �� -   $
 
textBlock1��$
��Left=�$��	233,7,0,0q�$
��Wrap=�$i��c2. Select the options you want to apply and hit Start!. By default, all options should be selected.��$	��Top=�$	��311��$��2��$��38���� -   $ Code_options��$��Code extraction options��$
��Left=�$	��106��$��10,7,0,0q�$��2��$	��Top=�$	��211��.���  �� $��26 ��� $��26 ��� $��26 �.+��� -    ��Name$ IsCamelCaseBox��$��camelCase splitting��$
��Left=�$��Center=�$��10,6,0,5q�$��0��$	��160��$��16�� d� 	IsChecked$
 TrueY��� -   $ IsUnderscoreBox��$��Underscore__splitting��$
��Left=�$��Center=�$��10,9,0,0q�$��1��$	��160��$��16��$
 TrueY��� -   $ IsCodeAsNaturalBox��$��Combine code and comments��$
��Left=�$��Center=�$��	10,10,0,1q�$��2��$	��179��$��16��$
 TrueY��� -   $��	StatusBar��$��Stretch=�$��32��$	��Top=�$��3��.@�� $	��544�� � ColumnDefinitions �� $
��457* �.+��� -   $
 StatusBlock��$��10,-10,10,-9q����� *	Message+ �� UpdateSourceTrigger$ PropertyChanged=�