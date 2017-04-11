import matplotlib.pylab as plt

# Temperature
temp_file = open("temp.txt")
data_rows = [x.split() for x in temp_file]
temp_years = [float(x[0]) for x in data_rows]  # TODO: change to time, ticks or whatever
temps = [float(x[1]) for x in data_rows]

# Moisture
moist_file = open("moist.txt")
data_rows = [x.split() for x in moist_file]
moist_years = [float(x[0]) for x in data_rows]
moists = [float(x[1]) for x in data_rows]

# Light
light_file = open("light.txt")
data_rows = [x.split() for x in light_file]
light_years = [float(x[0]) for x in data_rows]
lights = [float(x[1]) for x in data_rows]


x_from = 1990
x_to = 2005

base_ax = plt.axes()
plt.yticks([])
plt.xlim(x_from, x_to)
plt.xlabel("Time", size=16)
plt.title("Temperature, Light, and Moisture", size=16)

base_box = base_ax.get_position()
base_x = base_box.x0
base_y = base_box.y0
base_w = base_box.x1 - base_x
base_h = base_box.y1 - base_y

temp_ax = plt.axes([base_x, base_y + base_h*2/3., base_w, base_h/3.], frameon=False)
temp_ax.yaxis.tick_left()  # the y values are on the left
plt.plot(temp_years, temps, '#FF0000')
plt.xlim(x_from, x_to)
#plt.yticks(np.arange(20, 40, 5))  # from 20(C) to 40(C) with 5(C) step
plt.ylabel('Temperature (C)', size=14)
plt.xticks([])  # hide x values

moist_ax = plt.axes([base_x, base_y + base_h*1/3., base_w, base_h/3.], frameon=False)
moist_ax.yaxis.tick_right()  # the y values are on the right
moist_ax.yaxis.set_label_position("right")
#moist_ax.xaxis.tick_bottom()
plt.plot(moist_years, moists, '#0000FF')
plt.xlim(x_from, x_to)
plt.ylabel('Moisture', size=14)
plt.xticks([])  # hide x values

light_ax = plt.axes([base_x, base_y + base_h*0/3., base_w, base_h/3.], frameon=False)
temp_ax.yaxis.tick_left()  # the y values are on the left
plt.plot(light_years, lights, '#00FF00')
plt.xlim(x_from, x_to)
plt.ylabel('Light', size=14)
plt.xticks([])  # hide x values


# Legend
plt.axes(base_ax)
t_proxy = plt.Line2D([0], [0], c='#FF0000', lw=3)
l_proxy = plt.Line2D([0], [0], c='#00FF00', lw=3)
m_proxy = plt.Line2D([0], [0], c='#0000FF', lw=3)
plt.legend((t_proxy, l_proxy, m_proxy), ("Temperature", "Light", "Moisture"))

plt.savefig("output.svg")
plt.show()

