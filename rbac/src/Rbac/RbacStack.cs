using Amazon.CDK;
using Amazon.CDK.AWS.IAM;
using Constructs;

namespace Rbac
{
    public class RbacStack : Stack
    {
        internal RbacStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            //Admin role - full access
            var adminRole = new Role(this, "AdminRole", new RoleProps
            {
                RoleName = "AdminRole",
                AssumedBy = new AccountRootPrincipal(),
                ManagedPolicies = new[] { ManagedPolicy.FromAwsManagedPolicyName("AdministratorAccess") }
            });

            // Developer Role - Limited Access
            var developerRole = new Role(this, "DeveloperRole", new RoleProps
            {
                RoleName = "DeveloperRole",
                AssumedBy=new ServicePrincipal("ec2.amazonaws.com"),
                ManagedPolicies = new[]
                {
                    ManagedPolicy.FromAwsManagedPolicyName("AmazonS3FullAccess"),
                    ManagedPolicy.FromAwsManagedPolicyName("AWSLambda_FullAccess"),
                    ManagedPolicy.FromAwsManagedPolicyName("AmazonRDSFullAccess")
                }
            });

            // Read-Only Role
            var readOnlyRole = new Role(this, "ReadOnlyRole", new RoleProps
            {
                RoleName = "ReadOnlyRole",
                AssumedBy =new ServicePrincipal("ec2.amazonaws.com"),
                ManagedPolicies = new[]
                {
                    ManagedPolicy.FromAwsManagedPolicyName("ReadOnlyAccess")
                }
            });

            // IAM Groups
            var adminGroup = new Group(this, "AdminGroup", new GroupProps 
            {
                GroupName = "AdminGroup",
                ManagedPolicies = new[] { ManagedPolicy.FromAwsManagedPolicyName("AdministratorAccess") }
            });

            var developerGroup = new Group(this, "DeveloperGroup", new GroupProps
            {
                GroupName = "DeveloperGroup",
                ManagedPolicies = new[]
                {
                    ManagedPolicy.FromAwsManagedPolicyName("AmazonS3FullAccess"),
                    ManagedPolicy.FromAwsManagedPolicyName("AWSLambda_FullAccess"),
                    ManagedPolicy.FromAwsManagedPolicyName("AmazonRDSFullAccess")
                }
            });

            var readOnlyGroup = new Group(this, "ReadOnlyGroup", new GroupProps
            {
                GroupName = "ReadOnlyGroup",
                ManagedPolicies = new[]
                {
                    ManagedPolicy.FromAwsManagedPolicyName("ReadOnlyAccess")
                }
            });

            new CfnOutput(this, "AdminRoleOutput", new CfnOutputProps { Value = adminRole.RoleName });

        }
    }
}
