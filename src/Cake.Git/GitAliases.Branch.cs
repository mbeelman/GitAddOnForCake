﻿using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using Cake.Git.Extensions;
using LibGit2Sharp;
// ReSharper disable UnusedMember.Global

namespace Cake.Git
{
    /// <summary>
    /// Class GitAliases.
    /// </summary>
    public static partial class GitAliases
    {
        /// <summary>
        /// Gets the current branch.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="repositoryDirectoryPath">Path to repository.</param>
        /// <returns>GitBranch.</returns>
        /// <exception cref="ArgumentNullException">context
        /// or
        /// repositoryDirectoryPath</exception>
        /// <example>
        ///   <code>
        /// var repositoryDirectoryPath = DirectoryPath.FromString(".");
        /// var currentBranch = ((ICakeContext)cakeContext).GitBranchCurrent(repositoryDirectoryPath);
        ///   </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Branches")]
        public static GitBranch GitBranchCurrent(this ICakeContext context, DirectoryPath repositoryDirectoryPath)
        {
            if (context == null)
            {
                throw new InvalidOperationException($"Argument {nameof(context)} cannot be null!");
            }

            if (repositoryDirectoryPath == null)
            {
                throw new InvalidOperationException($"Argument {nameof(repositoryDirectoryPath)} cannot be null!");
            }

            return context.UseRepository(
                repositoryDirectoryPath,
                repository => new GitBranch(repository)
                );
        }

        /// <summary>
        /// Creates a local Branch.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="repositoryDirectoryPath">Path to repository.</param>
        /// <param name="branchName">Branch name to create.</param>
        /// <param name="checkOut">Option to checkout after branch creation.</param>
        /// <exception cref="ArgumentNullException">context
        /// or
        /// repositoryDirectoryPath
        /// or
        /// branchName</exception>
        /// <example>
        ///   <code>
        /// var repositoryDirectoryPath = DirectoryPath.FromString(".");
        /// var currentBranch = ((ICakeContext)cakeContext).GitCreateBranch(repositoryDirectoryPath, "FooBar", true);
        ///   </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Branch")]
        public static GitBranch GitCreateBranch(this ICakeContext context, DirectoryPath repositoryDirectoryPath, string branchName, bool checkOut)
        {
            if (context == null)
            {
                throw new InvalidOperationException($"Argument {nameof(context)} cannot be null!");
            }

            if (repositoryDirectoryPath == null)
            {
                throw new InvalidOperationException($"Argument {nameof(repositoryDirectoryPath)} cannot be null!");
            }

            if (string.IsNullOrWhiteSpace(branchName))
            {
                throw new InvalidOperationException($"Argument {nameof(branchName)} cannot be null!");
            }

            return context.UseRepository(repositoryDirectoryPath, 
                repository => 
                {
                    var localBranch = repository.CreateBranch(branchName);

                    if (checkOut)
                    {
                        Commands.Checkout(repository, localBranch);
                    }

                    return new GitBranch(repository, localBranch);
                });

        }
    }
}
