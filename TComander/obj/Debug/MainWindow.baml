   M S B A M L   `   `   `  ����D  @TComander, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:TComander	TComander  5   	       TComander.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:TComander     5   	     � Title$  
MainWindow��5   	   $	��700��6   $
��1300��6)   .��5	      � .+�5
   
   �� -   5         ��Name$ label_directoryLeft��5
      $��Directory: ��6-   $
��Left=�6C   $��	10,10,0,0q�6^   $	��Top=�6q   �� 5   
   -   5         ��Name$ textBox_directoryLeft��5      $
��Left=�61   $��23��6L   $��	79,14,0,0q�6X    �� TextWrapping$
 Wrap=�6k   $��C:\Users\Mindi\Desktop��6   $	��Top=�6�   $	��491��6�   �� 5   
   $
��Left=�6   $	��617��62   $��	10,42,0,0q�6?   $	��Top=�6R   $	��560��6j   .��5      R� -   5         R�Name$ treeView_directoryLeft��5      5      �� 5   
   $
��Left=�6   $	��617��62   $��
575,42,0,0q�6?   $	��Top=�6S   $	��560��6k   .��5      R� -   5        $ treeView_directoryRight��5      5      �� 5   
   -   5        $ label_directoryRight��5      $��Directory: ��6.   $
��Left=�6D   $��
575,10,0,0q�6_   $	��Top=�6s   �� 5   
   -   5        $ textBox_directoryRight��5      $
��Left=�62   $��23��6M   $��
643,14,0,0q�6Y   $
 Wrap=�6m   $��C:\Users\Mindi\Desktop��6�   $	��Top=�6�   $	��492��6�   �� 5   
   -   5         ��Name$ button_selec_description��5      $��Zaznaczone elementy��63   $
��Left=�6Q   $��1140,10,0,0q�6l   $	��Top=�6�   $	��142��6�   6�   �� 5   
   -   5        $ button_delet��5      $��Usuń��6'   $
��Left=�66   $��1140,76,0,0q�6Q   $	��Top=�6f   $	��142��6~   6�   �� 5   
   -	   5        $ button_move��5      $��	Przenieś��6&   $
��Left=�69   $��1140,101,0,0q�6T   $	��Top=�6j   $	��142��6�   6�   �� 5   
   -
   5        $ button_copy��5      $��Kopiuj��6&   $
��Left=�67   $��1140,126,0,0q�6R   $	��Top=�6h   $	��142��6�   6�   �� 5   
   -   5        $ button_deselect��5      $��Odznacz wszystko��6*   $
��Left=�6E   $��1140,35,0,0q�6`   $	��Top=�6u   $	��142��6�   6�   �� 5   
   -   5        $ button��5      $��Create Directory��6!   $
��Left=�6<   $��1140,174,0,0q�6W   $	��Top=�6m   $	��142��6�   6�   �� 5   
   -   5        $ label_sorting_group��5      $��Sortuj względem:��6-   $
��Left=�6H   $��1140,218,0,0q�6c   $	��Top=�6y   $	��142��6�   	� 5   
   -   5         	�Name$ radioButton_typeSort��5      $��Typów��64   $
��Left=�6D   $��1140,250,0,0q�6_   $	��Top=�6u    d� 	IsChecked$
 TrueY�6�   	� 5   
   -   5        $ radioButton_dataAscent��5      $��Daty utworzenia rosnąco��66   $
��Left=�6X   $��1140,270,0,0q�6s   $	��Top=�6�   6�   	� 5   
   -   5        $ radioButton_dataDescend��5      $��Daty utworzenia malejąco��67   $
��Left=�6Z   $��1140,290,0,0q�6u   $	��Top=�6�   6�   	� 5   
   -   5        $ radioButton_nameAscend��5      $��Nazwy rosnąco��66   $
��Left=�6N   $��1140,310,0,0q�6i   $	��Top=�6   6�   	� 5   
   -   5        $ radioButton_nameDescend��5      $��Nazwy malejąco��67   $
��Left=�6P   $��1140,330,0,0q�6k   $	��Top=�6�   6�   �� 5    
   -   5        $ label_search��5       $��	Wyszukaj:��6&   $
��Left=�6:   $��1140,375,0,0q�6U   $	��Top=�6k   �� 5!   
   -   5        $ textBox_search��5!      $
��Left=�6*   $��23��6E   $��1140,406,0,0q�6Q   $
 Wrap=�6g   $�� ��6{   $	��Top=�6�   $	��142��6�   �� 5"   
   -   5        $ button_search��5"      $��Wyszukaj��6(   $
��Left=�6;   $��1140,434,0,0q�6V   $	��Top=�6l   $	��142��6�   6�   5#      5$      