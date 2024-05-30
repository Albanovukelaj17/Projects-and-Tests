using Grpc.Core;
using HSRM.CS.DistributedSystems.Hamster.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HSRM.CS.DistributedSystems.Hamster
{
    internal class HamsterServer : HamsterService.HamsterServiceBase
    {
        private static IHamsterManagement lib = new HamsterManagement();

        public override Task<HamsterAddResponse> AddHamster(HamsterAddRequest request, ServerCallContext context)
        {
            if (string.IsNullOrEmpty(request.OwnerName) || string.IsNullOrEmpty(request.HamsterName))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument,
                    "Owner name or hamster name cannot be empty"));
            }

            try
            {
                var id = lib.NewHamster(request.OwnerName, request.HamsterName, (ushort)request.Treats);
                return Task.FromResult(new HamsterAddResponse { Id = id });
            }
            catch (HamsterNameTooLongException ex)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "the specified name is too long"));
            }
            catch (HamsterExistsException ex)
            {
                throw new RpcException(new Status(StatusCode.AlreadyExists,
                    "a hamster by that owner/name already exists"));
            }
            catch (HamsterStorageException ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, "storage error"));
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, "internal error"));
            }
        }

        public override Task<HamsterCollectResponse> Collect(HamsterCollectRequest request, ServerCallContext context)
        {
            if (string.IsNullOrEmpty(request.OwnerName))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Owner name cannot be empty"));
            }

            try
            {
                var treatsLeft = lib.Collect(request.OwnerName);
                Console.WriteLine($"The amount due for owner {request.OwnerName} is {treatsLeft} treats.");
                return Task.FromResult(new HamsterCollectResponse { TreatsLeft = treatsLeft });
            }
            catch (HamsterNotFoundException ex)
            {
                throw new RpcException(
                    new Status(StatusCode.NotFound, "A hamster or hamster owner could not be found."));
            }
            catch (HamsterStorageException ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, "storage error"));
            }
            catch (HamsterDatabaseCorruptedException ex)
            {
                throw new RpcException(new Status(StatusCode.DataLoss, "database is corrupted"));
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, "internal error"));
            }
        }

        public override Task<HamsterHowIsDoingResponse> HowIsDoing(HamsterHowIsDoingRequest request,
            ServerCallContext context)
        {
            try
            {
                var state = lib.Howsdoing(request.Id);
                return Task.FromResult(new HamsterHowIsDoingResponse
                {
                    Hamster = new HamsterHowIsDoingResponse.Types.HamsterState
                    {
                        Id = state.ID,
                        Cost = state.Cost,
                        Treats = state.TreatsLeft,
                        Rounds = state.Rounds
                    }
                });
            }
            catch (HamsterNotFoundException ex)
            {
                throw new RpcException(
                    new Status(StatusCode.NotFound, "A hamster or hamster owner could not be found."));
            }
            catch (HamsterStorageException ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, "storage error"));
            }
            catch (HamsterDatabaseCorruptedException ex)
            {
                throw new RpcException(new Status(StatusCode.DataLoss, "database is corrupted"));
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, "internal error"));
            }
        }

       public override Task<HamsterLookUpResponse> LookUp(HamsterLookUpRequest request, ServerCallContext context)
       {
           if (string.IsNullOrEmpty(request.OwnerName) || string.IsNullOrEmpty(request.HamsterName))
           {
               throw new RpcException(new Status(StatusCode.InvalidArgument,
                   "Owner name or hamster name cannot be empty"));
           }
       
           try
           {
               var id = lib.Lookup(request.OwnerName, request.HamsterName);
               return Task.FromResult(new HamsterLookUpResponse { Id = id });
           }
           catch (HamsterNotFoundException ex)
           {
               throw new RpcException(new Status(StatusCode.NotFound, "No hamsters matching criteria found"));
           }
           catch (HamsterStorageException ex)
           {
               throw new RpcException(new Status(StatusCode.Internal, "storage error"));
           }
           catch (HamsterDatabaseCorruptedException ex)
           {
               throw new RpcException(new Status(StatusCode.DataLoss, "database is corrupted"));
           }
           catch (Exception ex)
           {
               throw new RpcException(new Status(StatusCode.Internal, "internal error"));
           }
       }

     public override Task<HamsterGiveTreatsResponse> GiveTreats(HamsterGiveTreatsRequest request, ServerCallContext context)
     {
         // Check for negative treats
         if (request.Treats < 0)
         {
             // Log the error for debugging purposes
             Console.WriteLine($"Invalid number of treats: {request.Treats}. Number of treats cannot be negative.");
     
             // Throw an RpcException with InvalidArgument status
             throw new RpcException(new Status(StatusCode.InvalidArgument, "Number of treats cannot be negative"));
         }
     
         try
         {
             var treats = lib.GiveTreats(request.Id, (ushort)request.Treats);
             return Task.FromResult(new HamsterGiveTreatsResponse { Treats = treats });
         }
         catch (HamsterNotFoundException ex)
         {
             // Log the error for debugging purposes
             Console.WriteLine($"Hamster not found: {ex.Message}");
     
             // Throw an RpcException with NotFound status
             throw new RpcException(new Status(StatusCode.NotFound, "A hamster or hamster owner could not be found."));
         }
         catch (HamsterStorageException ex)
         {
             // Log the error for debugging purposes
             Console.WriteLine($"Storage error: {ex.Message}");
     
             // Throw an RpcException with Internal status
             throw new RpcException(new Status(StatusCode.Internal, "Storage error"));
         }
         catch (HamsterDatabaseCorruptedException ex)
         {
             // Log the error for debugging purposes
             Console.WriteLine($"Database corrupted: {ex.Message}");
     
             // Throw an RpcException with DataLoss status
             throw new RpcException(new Status(StatusCode.DataLoss, "Database is corrupted"));
         }
         catch (Exception ex)
         {
             // Log the error for debugging purposes
             Console.WriteLine($"Internal error: {ex.Message}");
     
             // Throw an RpcException with Internal status
             throw new RpcException(new Status(StatusCode.Internal, "Internal error"));
         }
     }


        public override Task<HamsterReadEntryResponse> ReadEntry(HamsterReadEntryRequest request,
            ServerCallContext context)
        {
            try
            {
                var treats = lib.ReadEntry(request.Id, out string ownerName, out string hamsterName, out ushort price);
                return Task.FromResult(new HamsterReadEntryResponse
                {
                    Treats = treats,
                    OwnerName = ownerName,
                    HamsterName = hamsterName,
                    Price = price
                });
            }
            catch (HamsterNotFoundException ex)
            {
                throw new RpcException(
                    new Status(StatusCode.NotFound, "A hamster or hamster owner could not be found."));
            }
            catch (HamsterStorageException ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, "storage error"));
            }
            catch (HamsterDatabaseCorruptedException ex)
            {
                throw new RpcException(new Status(StatusCode.DataLoss, "database is corrupted"));
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, "internal error"));
            }
        }

        public override Task<HamsterSearchResponse> Search(HamsterSearchRequest request, ServerCallContext context)
        {
            var response = new HamsterSearchResponse();
        
            try
            {
                var ownerName = string.IsNullOrEmpty(request.OwnerName) ? null : request.OwnerName;
                var hamsterName = string.IsNullOrEmpty(request.HamsterName) ? null : request.HamsterName;
        
                var ids = lib.Search(ownerName, hamsterName);
                response.Id.AddRange(ids);
            }
            catch (HamsterNameTooLongException ex)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "The hamster or owner name is too long."));
            } 
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, "Internal error: " + ex.Message));
            }
        
            return Task.FromResult(response);
        }
   }
}