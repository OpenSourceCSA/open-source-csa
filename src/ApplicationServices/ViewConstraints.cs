// using System;
// using System.Threading.Tasks;
// using Ezley.ProjectionStore;
// using Ezley.Shared.Exceptions;
//
// namespace ApplicationServices
// {
//     /// <summary>
//     /// This class contains functions for the Command side to use in checking
//     /// constraints using the Views. 
//     /// </summary>
//     public class ViewConstraints
//     {
//         private IViewRepository _repo;
//         public ViewConstraints(IViewRepository repo)
//         {
//             _repo = repo;
//         }
//
//         public async Task<bool> UserEmailIsUnique(string emailAddress)
//         {
//             var viewName = $"{nameof(UserByEmailView)}:{emailAddress}";
//             try
//             {
//                 var user = await _repo.LoadTypedViewAsync<UserByEmailView>(viewName);
//             }
//             catch (NotFoundException)
//             {
//                 return true;
//             }
//
//             return false;
//         }
//     }
// }