#include "mainwindow.h"
#include "ui_mainwindow.h"

#include <QDebug>
#include <QHostAddress>
#include <QAbstractSocket>
#include <QRandomGenerator>
#include <cmath>        // std::abs

MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow),
    _server(this)
{
    port = 4444;
    ui->setupUi(this);
    _server.listen(QHostAddress::Any, port);
    connect(&_server, SIGNAL(newConnection()), this, SLOT(onNewConnection()));
    connect(&_timer, SIGNAL(timeout()), this, SLOT(WritePointCloud()));
    _timer.start(100);
}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::onNewConnection()
{
   QTcpSocket *clientSocket = _server.nextPendingConnection();
   connect(clientSocket, SIGNAL(readyRead()), this, SLOT(onReadyRead()));
   connect(clientSocket, SIGNAL(stateChanged(QAbstractSocket::SocketState)), this, SLOT(onSocketStateChanged(QAbstractSocket::SocketState)));

    _sockets.push_back(clientSocket);
}

void MainWindow::onSocketStateChanged(QAbstractSocket::SocketState socketState)
{
    if (socketState == QAbstractSocket::UnconnectedState)
    {
        QTcpSocket* sender = static_cast<QTcpSocket*>(QObject::sender());
        _sockets.removeOne(sender);
    }
}

void MainWindow::onReadyRead()
{
    QTcpSocket* sender = static_cast<QTcpSocket*>(QObject::sender());
    QByteArray datas = sender->readAll();
    for (QTcpSocket* socket : _sockets) {
        if (socket != sender)
            socket->write(QByteArray::fromStdString(sender->peerAddress().toString().toStdString() + ": " + datas.toStdString()));
    }
}

void MainWindow::WritePointCloud()
{
    QByteArray _data;
    QVector<float> points;

    float numPoints = 5.0f;
    points.append(numPoints);

    int length = int(numPoints * 3); //150


    //max and min bounds to render point cloud around
    float zMin = 13.5f * 0.5f;
    float zMax = 13.5f;
    float yMax = 5.0f;
    float xMin = 5.5f * 0.5f;
    float xMax = 5.5f;

    for(int i = 0; i < length; i++)
    {
        //Generate random number from 0->1
        float x = float(QRandomGenerator::global()->generateDouble())* xMax;
        float y = float(QRandomGenerator::global()->generateDouble()) * yMax;
        float z = float(QRandomGenerator::global()->generateDouble()) * zMax;

        x *= QRandomGenerator::global()->generateDouble() > 0.5 ? 1 : -1;
        y *= QRandomGenerator::global()->generateDouble() > 0.5 ? 1 : -1;
        z *= QRandomGenerator::global()->generateDouble() > 0.5 ? 1 : -1;

        //Make sure that detection is not within the area of the vehicle (with 2m buffer)
        if((std::abs(x) > (2 + xMin)) || (std::abs(z) > (2 + zMin)))
        {
            x = 0.0;
            y = 0.0;
            z = 0.0;
        }
        points.append(x);
        points.append(y);
        points.append(z);
    }

    //Serialize points into QByteArray
    for (QTcpSocket* socket : _sockets) {
        socket->write(toByteArray(points));
    }
}

QByteArray MainWindow::toByteArray(QVector<float> data) {
    QByteArray bytes;
    bytes.clear();
    QDataStream stream(&bytes, QIODevice::WriteOnly);
    stream.setByteOrder(QDataStream::LittleEndian);

    QString result;

    for(int i = 0; i < data.length(); i++)
    {
        float f = data.at(i);

        QString str;
        if(i == 0) {
            str = QString("%1").arg(double(f), 0, 'f', 3);
        }
        else {
            str = QString(",%1").arg(double(f), 0, 'f', 3);
        }
        result.append(str);
    }

    bytes = QByteArray::fromStdString(result.toUtf8().constData());

    return bytes;
}
