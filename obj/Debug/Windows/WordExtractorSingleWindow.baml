   M S B A M L   `   `   `  ����B  >Program, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:SEEL.LinguisticProcessorSEEL.LinguisticProcessor  5   	   2    ,SEEL.LinguisticProcessor.WordExtractorWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   8local&clr-namespace:SEEL.LinguisticProcessor     5   	   -   5        $��WordExtractorW��5   	     � Title$  Word Extractor��6    $	��710��67   $��559.648��6D    � WindowState$ Normal=�6T    � 
ResizeMode$ CanMinimize=�6i   .��5	      �  � RowDefinitions 5
   
   �� 5      $��19 �6   �� 5      $	��157 �6   �� 5      $	��205 �6   �� 5      $	��268 �6   �� 5      $��61 �6   .+�5   
   o� -   5         o�Name$ Tools_mn��5      $
��Left=�6!   $��19��6<   $	��Top=�6H   $��41��6`   $��0��6k   .@�5      m� $��_Tools��6   .@�5      m� -   5        $��_Word Extractor��5      64   m� 5      -   5        $��_Linguistic Change��5      67   m� 5      $��_Link Extractor��6   m� 5      -   5        $��_Project Downloader��5      68   m� 5      -   5        $��_Projects Statistics��5      69   �� 5      m� 5      $��_Exit��6   5      5      �� 5   
   -   5         ��Name$ Source��5      $��Project��6#   $
��Left=�64   $	��141��6O   $��	10,10,0,0q�6\   $	��Top=�6o   $	��534��6�   $��1��6�   .��5      � $��	0,0,0,-12q�6    5       �� 5!      $��55 �6$   �� 5"      $��32 �6$   �� 5#      $��35 �6$   .+�5%      �� -   5         ��Name$ ProjectInputButton��5%      $��	Browse...��65   $
��Left=�6I   $��	10,26,0,0q�6d   $	��Top=�6w   $��75��6�   $��20��6�   $��0��6�   �� 5&      -	   5         ��Name$ ProjectPath��5&      $
��Left=�6/   $��21��6J   $��
111,25,0,0q�6V    �� TextWrapping$
 Wrap=�6j   $��Select project...��6~   
�������$	��Top=�6�   $	��401��6�   	 �� 
IsReadOnly	 . {�.  $��0��6�   �� 5'      -
   5        
 ��Name$
 	textBlock��5'      $
��Left=�6/   $��10,5,0,0q�6J   $
��Wrap=�6\   $"��1. Select the project folder��6p   $	��Top=�6�   $	��480��6�   �� 5(      -   5        $ ProcessWholeFolder_bt��5(      $��Process several projects��68   $
��Left=�6[   $��10,5,0,0q�6v   $	��Top=�6�   $	��135��6�   $��21��6�   $��2��6�   5)      5*      �� 5,   
   -   5        $ StartWordExtraction_bt��5,      $��Start!��61   $
��Left=�6B   $��358,142,0,0q�6]   $	��Top=�6r   $��75��6�   $��23��6�   $��2��6�   {�.  6�   �� 5-   
   -   5        $
 
textBlock1��5-      $
��Left=�6(   $��
233,11,0,0q�6C   $
��Wrap=�6W   $	��Top=�6k   $	��311��6�   $��2��6�   $	��113��6�   �� 6�   $���2. Select the options you want to apply and hit Start!. By default, all options should be selected. Calculate statistics will output the statistics for the project and releases once calculation is finished. ��6�   �� 6�  �� 6�  $7�1"Combine code and comments" merges words together��6�  6�  �� 5/   
   -   5        $ Code_options��5/      $��Code extraction options��6)   $
��Left=�6J   $	��100��6e   $��	10,11,0,0q�6r   $��2��6�   $	��Top=�6�   $	��211��6�   .��50      �  51      �� 52      $��26 �6$   �� 53      $��26 �6$   �� 54      $��26 �6$   �� 55      $��26 �6$   �� 56      $��26 �6$   .+�58      �� -   5         ��Name$ IsCalculateStatisticsBox��58      $��Calculate statistics��6=   $
��Left=�6\   $��Center=�6w   $��10,6,0,4q�6�   $��0��6�   $	��116��6�   $��16��6�    d� 	IsChecked$
 TrueY�6�   6  �� 59      -   5        $ IsCamelCaseBox��59      $��camelCase splitting��63   $
��Left=�6Q   $��Center=�6l   $��10,6,0,4q�6�   $��1��6�   $	��160��6�   $��16��6�   $
 TrueY�6�   6�   �� 5:      -   5        $ IsUnderscoreBox��5:      $��Underscore__splitting��64   $
��Left=�6T   $��Center=�6o   $��10,6,0,4q�6�   $��2��6�   $	��160��6�   $��16��6�   $
 TrueY�6�   6�   5;      5<      �� 5>   
   -   5         ��Name$ 	OutputTab��5>      $��Stretch=�6(   $��Stretch=�6F   $��3��6b   $��0,0,0,1q�6o   6�   �� 5@   
   -   5        $��	StatusBar��5@      $��Stretch=�6%   $��24��6C   $	��Top=�6O   $��4��6g   .@�5A      � $	��544��6    � ColumnDefinitions 5B      �� 5C      $	��69* �6'   �� 5D      $
��457* �6'   .+�5F      �� -   5        $
 StatusBlock��5F      $��5,-12,10,-22q�61   $��2��6�   ��6G   �� *	Message+ �� UpdateSourceTrigger$ PropertyChanged=�6�   5G      5H      �� 5J   
   -   5        $��StatisticsBar��5J      $��Stretch=�6)   $��32��6G   $	��Top=�6S   $��4��6k   .@�5K      � $	��526��6    5L      �� 5M      $	��85* �6'   �� 5N      $
��178* �6'   .+�5P      �� $��Files:��6   $��0��6*   $
��Left=�6:   $��26��6U   �� 5Q      -   5        $��
FilesTotal��5Q      $��31,0,0,0q�6o   $��0��6�   $
��Left=�6�   $��70��6�   ��6.   �� *
FilesTotal+$ PropertyChanged=�6�   �� 5R      $
��LOC:��6   $��0,0,40,0q�6(   $��0��6:   $��Right=�6J   $��24��6f   �� 5S      -   5        $��TotalLOC��5S      $��	0,0,313,0q�6n   $��0��6�   $��Right=�6�   $��79��6�   $��2��6�   ��6,   �� *LinesOfCode+$ PropertyChanged=�6�   �� 5T      	 �� X1$ 0��6   	 �� X2$ 0��6   	 �� Y1$ 0��6%   	 �� Y2$ 20��6,   
�������$��1��6B   $��
48,0,271,0q�6V   $��1��6j   �� 5U      $��Count:��6   $��
56,0,257,0q�6*   $��1��6>   �� 5V      -   5        $��NumberOfWords��5V      $��104,0,183,0q�6u   $��1��6�   ��61   �� *NumberOfWords+$ PropertyChanged=�6�   5W      5X      �� 5Y   
   -   5         ��Name$ Releases_cb��5Y      $
��Left=�6(   $��	439,0,0,0q�6C   $��3��6V   $	��Top=�6c   $	��105��6{   $
��Auto=�6�   $��22��6�   5Z      5[      