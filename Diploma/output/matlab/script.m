omega1 = dlmread('D:\Develop\Diploma\Diploma\output\omega\omega1.txt');
omega2 = dlmread('D:\Develop\Diploma\Diploma\output\omega\omega2.txt');
omega3 = dlmread('D:\Develop\Diploma\Diploma\output\omega\omega3.txt');
time = dlmread('D:\Develop\Diploma\Diploma\output\time.txt');
plot(time, omega1, time, omega2, time, omega3);