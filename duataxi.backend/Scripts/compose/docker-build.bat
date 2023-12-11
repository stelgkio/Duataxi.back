 cd C:\Users\steli\Documents\Projects\DuaTaxi\easytravel.identity
     
     
     docker build -f .\DuaTaxi.CoreApi\Dockerfile -t  duataxi.coreapi:v1 .
     docker build -f .\AuthServer\Dockerfile -t  duataxi.auth:v1 .
     docker build -f .\DuaTaxi.Operations\Dockerfile -t  duataxi.operations:v1 .


     docker build -f .\DuaTaxi.TaxiApi\Dockerfile -t  duataxi.taxiapi:v1 .
     docker build -f .\DuaTaxi.Service.MiniBusApi\Dockerfile -t  duataxi.minibusapi:v1 .
     docker build -f .\DuaTaxi.Service.BusApi\Dockerfile -t  duataxi.basapi:v1 .
     
     docker build -f .\DuaTaxi.Service.Payments\Dockerfile -t  duataxi.payments:v1 .
     
     docker build -f .\DuaTaxi.Service.FileApi\Dockerfile -t  duataxi.fileapi:v1 .
     docker build -f .\DuaTaxi.SMTP\Dockerfile -t  duataxi.smtp:v1 .
     
     docker build -f .\DuaTaxi.ErrorHandlerSignalr\Dockerfile -t  duataxi.errorhandlersignalr:v1 .


    REM  docker run -p 5555:5555 --network=duataxi-network  duataxi.coreapi:v1        
    REM  docker run -p 5000:5000 --network=duataxi-network  duataxi.auth:v1           
    REM  docker run -p 5004:5004 --network=duataxi-network  duataxi.operations:v1:v1        

    REM docker run -p 5010:5010 --network=duataxi-network  duataxi.taxiapi:v1       
    REM docker run -p 5006:5006 --network=duataxi-network  duataxi.minibusapi:v1
    REM docker run -p 5005:5005 --network=duataxi-network  duataxi.busapi:v1

    REM docker run -p 5002:5002 --network=duataxi-network  duataxi.payments:v1   

    REM docker run -p 5012:5012 --network=duataxi-network  duataxi.fileapi:v1       
    REM docker run -p 5011:5011 --network=duataxi-network  duataxi.smtp:v1    

    REM docker run -p 5007:5007 --network=duataxi-network  duataxi.errorhandlersignalr:v1    
