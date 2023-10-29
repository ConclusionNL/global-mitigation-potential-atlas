Aniq on the data files:

More data points, 10 points per collaboration for the total_data and combined_data.
More technologies.
Autarky_emissions and autarky_costs in the combined_data.csv that simulates the difference between autarky and collaboration. This is so that it can be used for the pop-up in the map view.
Included examples where the minimum collaboration_emissions (or autarky_emissions) are more than zero. I.e. the graph does not reach zero in the x-axis.



on heatmaps.csv (used for the coloring in the worldmap):
The file now has columns labelled by numbers. These numbers represent a value in a slider. When users move a slider, the heatmap should ideally swap data from one column to the respective column. This is for the “A slider to change the mitigation cost for the heatmap”