   M S B A M L   `   `   `  ����B  >Program, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:SEEL.LinguisticProcessorSEEL.LinguisticProcessor  5   	   =    7SEEL.LinguisticProcessor.LinguisticChangeMultipleWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   8local&clr-namespace:SEEL.LinguisticProcessor     5   	   -   5        $��LinguisticChangeW��5   	     � Title$  Linguistic Change��6"   $��454.22��6<   $��559.648��6L    � WindowState$ Normal=�6\    � 
ResizeMode$ CanMinimize=�6q   .��5	      �  � RowDefinitions 5
   
   �� 5      $��19 �6   �� 5      $	��127 �6   �� 5      $	��124 �6   �� 5      $	��124 �6   �� 5      $��95 �6   .+�5   
   o� -   5         o�Name$ Tools_mn��5      $
��Left=�6!   $��19��6<   $	��Top=�6H   $��41��6`   $��0��6k   .@�5      m� $��_Tools��6   .@�5      m� -   5        $��_Word Extractor��5      64   m� 5      -   5        $��_Linguistic Change��5      67   m� 5      $��_Link Extractor��6   m� 5      $��_Project Downloader��6   m� 5      -   5        $��_Projects Statistics��5      69   �� 5      m� 5      $��_Exit��6   5      5      �� 5   
   -   5         ��Name$ Source��5      $@��:Linguistic Change 1st and last releases: multiple projects��6#   $
��Left=�6g   $	��107��6�   $��	10,10,0,0q�6�   $	��Top=�6�   $	��534��6�   $��1��6�   .��5      � .+�5       �� -   5         ��Name$ FirstAndLastRelease_bt��5       $��	Browse...��69   $
��Left=�6M   $��	10,26,0,0q�6h   $	��Top=�6{   $��75��6�   $��19��6�   �� 5!      -   5         ��Name$ FolderProjects��5!      $
��Left=�62   $��20��6M   $��
111,25,0,0q�6Y    �� TextWrapping$
 Wrap=�6m   $��Select folder(s)...��6�   
�������$	��Top=�6�   $	��401��6�   	 �� 
IsReadOnly	 . {�.  6�   �� 5"      -	   5        
 ��Name$
 	textBlock��5"      $
��Left=�6/   $��10,5,0,0q�6J   $
��Wrap=�6\   $(��"Select the folders of the projects��6p   $	��Top=�6�   $	��480��6�   5#      5$      �� 5&   
   -
   5        $( "SeveralProjectsStartCalculation_bt��5&      $��
Calculate!��6=   $
��Left=�6R   $��
451,87,0,0q�6m   $	��Top=�6�   $��75��6�   $��20��6�   $��1��6�   {�.  6�   �� 5(   
   -   5        $��	StatusBar��5(      $��Stretch=�6%   $��24��6C   $	��Top=�6O   $��4��6g   .@�5)      � $	��544��6    � ColumnDefinitions 5*      �� 5+      $	��69* �6'   �� 5,      $
��457* �6'   .+�5.      �� -   5        $
 StatusBlock��5.      $��5,0,10,0q�61   $��1��6[   ��6C   �� *Status+6[   �� 5/      -   5        $
 Progress��5/      ��6.   �� *
Progress+50      51      �� 52   
   -   5        $ Source_Copy��52      $8��2Interquartile Linguistic Change: multiple projects��6(   $
��Left=�6d   $	��107��6   $��	10,10,0,0q�6�   $	��Top=�6�   $	��534��6�   $��2��6�   .��53      � .+�54      �� -   5        $ Interquartile_bt��54      $��	Browse...��63   $
��Left=�6G   $��	10,26,0,0q�6b   $	��Top=�6u   $��75��6�   $��19��6�   �� 55      -   5        $ FolderProjects1��55      $
��Left=�63   $��20��6N   $��
111,25,0,0q�6Z   $
 Wrap=�6n   $��Select folder(s)...��6�   
�������$	��Top=�6�   $	��401��6�   	 . {�.  6�   �� 56      -   5        $
 
textBlock1��56      $
��Left=�60   $��10,5,0,0q�6K   $
��Wrap=�6]   $(��"Select the folders of the projects��6q   $	��Top=�6�   $	��480��6�   57      58      �� 59   
   -   5         ��Name$ IsAverageBt��59      $��Calculate the averages��6(   $
��Left=�6I   $��	27,89,0,0q�6d   $��2��6w   $	��Top=�6�   $��15��6�   $	��139��6�   6�   �� 5:   
   -   5        $&  InterquartileStartCalculation_bt��5:      $��
Calculate!��6;   $
��Left=�6P   $��
451,86,0,0q�6k   $	��Top=�6   $��75��6�   $��20��6�   $��2��6�   {�.  6�   �� 5;   
   -   5        $ Source_Copy1��5;      $2��,Average Linguistic Change: multiple projects��6)   $
��Left=�6_   $	��107��6z   $��	10,12,0,0q�6�   $	��Top=�6�   $	��534��6�   $��3��6�   .��5<      � .+�5=      �� -   5        $ Survival_bt��5=      $��	Browse...��6.   $
��Left=�6B   $��	10,26,0,0q�6]   $	��Top=�6p   $��75��6�   $��19��6�   �� 5>      -   5        $ FolderProjects2��5>      $
��Left=�63   $��20��6N   $��
111,25,0,0q�6Z   $
 Wrap=�6n   $��Select folder(s)...��6�   
�������$	��Top=�6�   $	��401��6�   	 . {�.  6�   �� 5?      -   5        $
 
textBlock2��5?      $
��Left=�60   $��10,5,0,0q�6K   $
��Wrap=�6]   $(��"Select the folders of the projects��6q   $	��Top=�6�   $	��480��6�   	� 5@      -   5         	� 	GroupName$ WordsOrLengthsSelector��5@      $��By word types��6A   $
��Left=�6Y   $��	10,60,0,0q�6t   $	��Top=�6�    d� 	IsChecked$
 TrueY�6�   	� 5A      -   5        $ WordsOrLengthsSelector��5A      $��By word lengths��6A   $
��Left=�6[   $��
111,60,0,0q�6v   $	��Top=�6�   6�   5B      5C      �� 5D   
   -   5        $! SurvivalStartCalculation_bt��5D      $��
Calculate!��66   $
��Left=�6K   $��
451,82,0,0q�6f   $	��Top=�6z   $��75��6�   $��20��6�   $��3��6�   {�.  6�   5E      5F      