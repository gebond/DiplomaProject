omega1 = dlmread('D:\Develop\Diploma\Diploma\output\omega\omega1.txt', ';');
omega2 = dlmread('D:\Develop\Diploma\Diploma\output\omega\omega2.txt', ';');
omega3 = dlmread('D:\Develop\Diploma\Diploma\output\omega\omega3.txt', ';');
time = dlmread('D:\Develop\Diploma\Diploma\output\time.txt', ';');


lambda0 = dlmread('D:\Develop\Diploma\Diploma\output\lambda\lambda0.txt', ';');
lambda1 = dlmread('D:\Develop\Diploma\Diploma\output\lambda\lambda1.txt', ';');
lambda2 = dlmread('D:\Develop\Diploma\Diploma\output\lambda\lambda2.txt', ';');
lambda3 = dlmread('D:\Develop\Diploma\Diploma\output\lambda\lambda3.txt', ';');

plot(time, omega1, time, omega2, time, omega3);
plot(time, lambda0, time, lambda1, time, lambda2,time, lambda3);