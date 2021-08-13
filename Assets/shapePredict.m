%shapePredict.m

%NOTES: Ensure this script is in the same directory
% as the snapshots (Ex: "snap_1.png") and anet.mat
% Outputs are written to "predictedShapes.txt" with
% each shapes on a newline

function shapePredict()

clear 
close all;

%When called, sequentially iterate through all segmented screenshots,
% and output the predicted shape into text file "prediction.txt" 

%Variables expressing how the main webcam image is segmented
rowNum = 2;
colNum = 3;

%Load classifier
load('anet.mat');
net = anet;
imageSize = net.Layers(1).InputSize;

%For Testing
%analyzeNetwork(anet)
string = "";
%Open classifier output file

for i = 1:(rowNum*colNum)
    filename ="Snapshots\"+ "snap_" + i + ".png";
    %disp(filename);
    image = imread(fullfile(filename));
    augmentedTest = augmentedImageDatastore(imageSize, image, 'ColorPreprocessing', 'gray2rgb');
    [predict,scores] = classify(net,augmentedTest);
    if(predict == "Square")
        predict = "0";
    elseif(predict == "Circle")
        predict = "1";
    elseif(predict == "Triangle") 
        predict = "2";
    elseif(predict == "Pentagon") 
        predict = "3";
    elseif(predict == "Star") 
        predict = "4";
    end
    string = string + predict + newline;
    %fprintf(fileID, char(predict));
    %fprintf(fileID, "\n");
%myString = predict + "1" + newline;
end
fileID = fopen('predictedShapes.txt', 'w');
fprintf(fileID, char(string));
fclose(fileID);

% Test Block
% augmentedTest = augmentedImageDatastore(imageSize, image, 'ColorPreprocessing', 'gray2rgb');
% [predict,scores] = classify(net,augmentedTest);
% fprintf(fileID, char(predict));
% fclose(fileID);
